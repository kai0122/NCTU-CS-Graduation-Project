#if ENABLE_UNET
using UnityEditor;
using UnityEngine;
using System.Collections;
using System.Globalization;
using UnityEngine.Assertions.Must;
using UnityEngine.Networking;


namespace UnityEditor
{
	[CustomEditor(typeof(NetworkSkeleton), true)]
	[CanEditMultipleObjects]
	public class NetworkSkeletonEditor : Editor
	{
		private static string[] axisOptions = { "None", "X", "Y (Top-Down 2D)", "Z (Side-on 2D)", "XY (FPS)", "XZ", "YZ", "XYZ (full 3D)" };

		bool m_Initialized = false;
		NetworkSkeleton sync;

		SerializedProperty m_Target;

		SerializedProperty m_InterpolateRotation;
		SerializedProperty m_InterpolateMovement;
		SerializedProperty m_RotationSyncCompression;
		SerializedProperty m_SyncLevel;
		SerializedProperty m_NetworkChannel;

		protected GUIContent m_InterpolateRotationLabel;
		protected GUIContent m_InterpolateMovementLabel;
		protected GUIContent m_RotationSyncCompressionLabel;
		protected GUIContent m_SyncLevelLabel;
		protected GUIContent m_NetworkChannelLabel;

		SerializedProperty m_NetworkSendIntervalProperty;
		GUIContent m_NetworkSendIntervalLabel;

		bool showBones;

		public void Init()
		{
			if (m_Initialized)
				return;

			m_Initialized = true;
			sync = target as NetworkSkeleton;

			m_Target = serializedObject.FindProperty("m_Target");
			if (sync.GetComponent<NetworkTransform>() == null)
			{
				if (LogFilter.logError) { Debug.LogError("NetworkSkeleton must be on the root object with the NetworkTransform, not on the child node"); }
				m_Target.objectReferenceValue = null;
			}


			m_InterpolateRotation = serializedObject.FindProperty("m_InterpolateRotation");
			m_InterpolateMovement = serializedObject.FindProperty("m_InterpolateMovement");
			m_RotationSyncCompression = serializedObject.FindProperty("m_RotationSyncCompression");
			m_SyncLevel = serializedObject.FindProperty("m_SyncLevel");
			m_NetworkChannel = serializedObject.FindProperty("m_NetworkChannel");

			m_NetworkSendIntervalProperty = serializedObject.FindProperty("m_SendInterval");
			m_NetworkSendIntervalLabel = new GUIContent("Network Send Rate (seconds)", "Number of network updates per second");
			EditorGUI.indentLevel += 1;

			m_InterpolateRotationLabel = new GUIContent("Interpolate Rotation Factor");
			m_InterpolateMovementLabel = new GUIContent("Interpolate Movement Factor");
			m_RotationSyncCompressionLabel = new GUIContent("Compress Rotation");
			m_SyncLevelLabel = new GUIContent("SyncLevel", "The number of levels of skeleton heirarchy to synchronize. Zero means synchronize all bones.");
			m_NetworkChannelLabel = new GUIContent("NetworkChannel");

			EditorGUI.indentLevel -= 1;
		}

		protected void ShowControls()
		{
			if (m_Target == null)
			{
				m_Initialized = false;
			}
			Init();

			serializedObject.Update();

			int sendRate = 0;
			if (m_NetworkSendIntervalProperty.floatValue != 0)
			{
				sendRate = (int)(1 / m_NetworkSendIntervalProperty.floatValue);
			}
			int newSendRate = EditorGUILayout.IntSlider(m_NetworkSendIntervalLabel, sendRate, 0, 30);
			if (newSendRate != sendRate)
			{
				if (newSendRate == 0)
				{
					m_NetworkSendIntervalProperty.floatValue = 0;
				}
				else
				{
					m_NetworkSendIntervalProperty.floatValue = 1.0f / newSendRate;
				}
			}
			if (EditorGUILayout.PropertyField(m_Target))
			{
				if (sync.GetComponent<NetworkTransform>() == null)
				{
					if (LogFilter.logError) { Debug.LogError("NetworkSkeleton must be on the root object with the NetworkTransform, not on the child node"); }
					m_Target.objectReferenceValue = null;
				}
			}

			EditorGUILayout.PropertyField(m_InterpolateMovement, m_InterpolateMovementLabel);

			EditorGUILayout.PropertyField(m_InterpolateRotation, m_InterpolateRotationLabel);

			int newRotation = EditorGUILayout.Popup(
				"Rotation Axis",
				(int)sync.syncRotationAxis,
				axisOptions);
			if ((NetworkTransform.AxisSyncMode)newRotation != sync.syncRotationAxis)
			{
				sync.syncRotationAxis = (NetworkTransform.AxisSyncMode)newRotation;
				EditorUtility.SetDirty(sync);
			}

			EditorGUILayout.PropertyField(m_RotationSyncCompression, m_RotationSyncCompressionLabel);
			EditorGUILayout.PropertyField(m_SyncLevel, m_SyncLevelLabel);
			EditorGUILayout.PropertyField(m_NetworkChannel, m_NetworkChannelLabel);

			serializedObject.ApplyModifiedProperties();

			if (!Application.isPlaying)
			{
				return;
			}

			EditorGUILayout.IntField("Number of Bones", sync.numBones);
			EditorGUILayout.IntField("Binary Size", sync.binarySize);

			if (sync.m_BoneInfos != null)
			{
				showBones = EditorGUILayout.Foldout(showBones, "Bones:");
				if (showBones)
				{
					foreach (var b in sync.m_BoneInfos)
					{
						EditorGUILayout.TextField(b.m_bone.gameObject.name, b.m_TargetSyncRotation3D.ToString());
					}
				}
			}
		}

		public override void OnInspectorGUI()
		{
			ShowControls();
		}
	}
}
#endif //ENABLE_UNET
