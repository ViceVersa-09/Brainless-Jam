using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(CameraController))]
public class CameraControllerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        CameraController cameraController = (CameraController)target;
        base.OnInspectorGUI();

        if (GUILayout.Button("TestCameraShake"))
        {
            cameraController.PlayCameraShakeRoutine(cameraController.testShakeDuration, cameraController.testShakeMagnitude);
        }
    }
}
