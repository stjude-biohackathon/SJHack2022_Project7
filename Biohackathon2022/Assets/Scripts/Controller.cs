using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Controller : MonoBehaviour
{    
    public GameObject BaseAssembly = null;
    public GameObject MidAssembly = null;
    public GameObject CouchAssembly = null;

    public enum Target { BaseAssembly, MidAssembly, CouchAssembly };
    public enum Action { NoOp, Translate, Rotate };

    public class Instruction
    {
        public Target target;
        public Action actionType;
        public float durationSeconds;

        // Internal
        public float t0;
        public Vector3 initialPosition, finalPosition;
        public Quaternion initialRotation, finalRotation;

        public Instruction(
            Target target,
            Action actionType,
            float x, float y, float z,
            float durationSeconds)
        {
            this.target = target;
            this.actionType = actionType;
            this.durationSeconds = durationSeconds;

            switch (actionType)
            {
                case Action.Translate:
                    this.finalPosition = new Vector3(x,y,z);
                    break;

                case Action.Rotate:
                    this.finalRotation = Quaternion.Euler(x,y,z);
                    break;
            }

            this.t0 = -1f;
            this.initialPosition = Vector3.zero;
            this.initialRotation = Quaternion.identity;
        }
    };

    List<List<Instruction>> instructionGroups = new List<List<Instruction>>();

    // Start is called before the first frame update
    void Start()
    {
        //instructions.Add(new Instruction(Target.BaseAssembly, Motion.Translate, new Vector3(0,1,0), 5f) );

        var ig = new List<Instruction>() {
            new Instruction(Target.BaseAssembly, Action.Translate, 0,-0.35f,0, 5f),
            new Instruction(Target.BaseAssembly, Action.Rotate, 0,180,0, 5f),
            new Instruction(Target.MidAssembly, Action.Rotate, 0,180,0, 5f),
            new Instruction(Target.CouchAssembly, Action.Rotate, 0,180,0, 5f),
        };
        instructionGroups.Add(ig);

        ig = new List<Instruction>() {
            new Instruction(Target.BaseAssembly, Action.NoOp, 0,0,0, 2f),
        };
        instructionGroups.Add(ig);

        ig = new List<Instruction>() {
            new Instruction(Target.BaseAssembly, Action.Translate, 0,0,0, 5f),
            new Instruction(Target.BaseAssembly, Action.Rotate, 0,0,0, 5f),
            new Instruction(Target.MidAssembly, Action.Rotate, 0,0,0, 5f),
            new Instruction(Target.CouchAssembly, Action.Rotate, 0,0,0, 5f),
        };
        instructionGroups.Add(ig);
    }

    // Update is called once per frame
    void Update() 
    {
        
    }

    GameObject GetTargetGameObject(Target t)
    {
        switch(t)
        {
            case Target.BaseAssembly:
                return BaseAssembly;

            case Target.MidAssembly:
                return MidAssembly;

            case Target.CouchAssembly:
                return CouchAssembly;
        }

        return null;
    }

    void FixedUpdate()
    {
        if (instructionGroups.Count < 1) return;

        var t = Time.time;
        var ig = instructionGroups[0];

        for( var i=0; i<ig.Count; i++ )
        {
            var instruction = ig[i];
            var target = GetTargetGameObject(instruction.target);

            if (instruction.t0 < 0)
            {
                instruction.t0 = t;
                instruction.initialPosition = target.transform.localPosition;
                instruction.initialRotation = target.transform.localRotation;
            }

            var u = (t-instruction.t0)/instruction.durationSeconds;
            if (u > 1f) u = 1f;

            switch(instruction.actionType)
            {
                case Action.NoOp:
                break;

                case Action.Translate:
                    {
                    var a = instruction.initialPosition;
                    var b = instruction.finalPosition;
                    target.transform.localPosition = Vector3.Lerp(a,b,u);
                    //Debug.Log($"{instruction}: {a} {b} {u} ({t})");
                    }
                break;

                case Action.Rotate:
                    {
                    var a = instruction.initialRotation;
                    var b = instruction.finalRotation;
                    target.transform.localRotation = Quaternion.Lerp(a,b,u);
                    //Debug.Log($"{instruction}: {a} {b} {u} ({t})");
                    }
                break;
            }
        }

        // Remove any completed instructions from current group
        ig.RemoveAll( x => (x.t0 + x.durationSeconds) < t);

        // Remove fully completed group from stack
        if (ig.Count < 1)
        {
            Debug.LogWarning("Removing previously active instruction group.");
            instructionGroups.RemoveAt(0);
        }
    }

    private void OnTriggerEnter(Collider other){
        Debug.Log("Hits detected");
    }
}
