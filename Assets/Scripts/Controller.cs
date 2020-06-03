using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Controller : MonoBehaviour {
    public Transform ballPrefab;
    public Transform targetPrefab;
    public Transform bouncer;
    public Vector2 inputSensitivity = Vector2.one / 2f;
    private Vector2 pitchAndRollChange;
    private Vector2 platformPitchAndRoll = Vector2.zero;
    private Color[] colors = {Color.red, Color.green, Color.blue, Color.white};
    private Transform[] targets = new Transform[4];
    private float dropInterval = 4;
    private float nextDrop;

    // Start is called before the first frame update
    void Start() {
        nextDrop = Time.time + dropInterval;
        Vector3[] offsets = {Vector3.left, Vector3.right, Vector3.back, Vector3.forward} ;
        for (int i = 0; i < offsets.Length; i++) {
            var t = Instantiate(targetPrefab);
            var m = t.GetComponent<Renderer>().material;
            m.color = colors[i];
            t.position = offsets[i] * 4;
            targets[i] = t;
        }
    }

    // Update is called once per frame
    void Update() {
        if (Time.time > nextDrop) {
            nextDrop = Time.time + dropInterval;
            ShrinkTargets();
            platformPitchAndRoll = Vector2.zero;
            var ball = Instantiate(ballPrefab);
            var m = ball.GetComponent<Renderer>().material;
            m.color = colors[Random.Range(0, colors.Length)];
        }

        platformPitchAndRoll += pitchAndRollChange;
        bouncer.rotation = Quaternion.Euler(platformPitchAndRoll.y, 0, -platformPitchAndRoll.x);
    }

    private void ShrinkTargets() {
        foreach (Transform target in targets) {
            var tl = target.localScale;
            target.localScale = new Vector3(tl.x * 0.95f, tl.y, tl.z * 0.95f);
        }
    }

    public void OnLook(InputAction.CallbackContext context) => pitchAndRollChange = 
        context.ReadValue<Vector2>() * inputSensitivity;
}
