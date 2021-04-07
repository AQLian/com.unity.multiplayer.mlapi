using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace MLAPI.Editor
{
    [CustomEditor(typeof(NetworkObject), true)]
    [CanEditMultipleObjects]
    public class NetworkObjectEditor : UnityEditor.Editor
    {
        private bool m_Initialized;
        private NetworkObject m_NetworkObject;
        private bool m_ShowObservers;

        private void Initialize()
        {
            if (m_Initialized)
            {
                return;
            }

            m_Initialized = true;
            m_NetworkObject = (NetworkObject)target;
        }

        public override void OnInspectorGUI()
        {
            Initialize();

            if (!m_NetworkObject.IsSpawned && NetworkManager.Singleton != null && NetworkManager.Singleton.IsServer)
            {
                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.LabelField(new GUIContent("Spawn", "Spawns the object across the network"));
                if (GUILayout.Toggle(false, "Spawn", EditorStyles.miniButtonLeft))
                {
                    m_NetworkObject.Spawn();
                    EditorUtility.SetDirty(target);
                }

                EditorGUILayout.EndHorizontal();
            }
            else if (m_NetworkObject.IsSpawned)
            {
                var guiEnabled = GUI.enabled;
                GUI.enabled = false;
                EditorGUILayout.TextField(nameof(NetworkObject.GlobalObjectIdHash), m_NetworkObject.GlobalObjectIdHash.ToString("X"));
                EditorGUILayout.TextField(nameof(NetworkObject.NetworkObjectId), m_NetworkObject.NetworkObjectId.ToString());
                EditorGUILayout.TextField(nameof(NetworkObject.OwnerClientId), m_NetworkObject.OwnerClientId.ToString());
                EditorGUILayout.Toggle(nameof(NetworkObject.IsSpawned), m_NetworkObject.IsSpawned);
                EditorGUILayout.Toggle(nameof(NetworkObject.IsLocalPlayer), m_NetworkObject.IsLocalPlayer);
                EditorGUILayout.Toggle(nameof(NetworkObject.IsOwner), m_NetworkObject.IsOwner);
                EditorGUILayout.Toggle(nameof(NetworkObject.IsOwnedByServer), m_NetworkObject.IsOwnedByServer);
                EditorGUILayout.Toggle(nameof(NetworkObject.IsPlayerObject), m_NetworkObject.IsPlayerObject);
                if (m_NetworkObject.IsSceneObject.HasValue)
                {
                    EditorGUILayout.Toggle(nameof(NetworkObject.IsSceneObject), m_NetworkObject.IsSceneObject.Value);
                }
                else
                {
                    EditorGUILayout.TextField(nameof(NetworkObject.IsSceneObject), "null");
                }
                EditorGUILayout.Toggle(nameof(NetworkObject.DestroyWithScene), m_NetworkObject.DestroyWithScene);
                GUI.enabled = guiEnabled;

                if (NetworkManager.Singleton != null && NetworkManager.Singleton.IsServer)
                {
                    m_ShowObservers = EditorGUILayout.Foldout(m_ShowObservers, "Observers");

                    if (m_ShowObservers)
                    {
                        HashSet<ulong>.Enumerator observerClientIds = m_NetworkObject.GetObservers();

                        EditorGUI.indentLevel += 1;

                        while (observerClientIds.MoveNext())
                        {
                            if (NetworkManager.Singleton.ConnectedClients[observerClientIds.Current].PlayerObject != null)
                            {
                                EditorGUILayout.ObjectField($"ClientId: {observerClientIds.Current}", NetworkManager.Singleton.ConnectedClients[observerClientIds.Current].PlayerObject, typeof(GameObject), false);
                            }
                            else
                            {
                                EditorGUILayout.TextField($"ClientId: {observerClientIds.Current}", EditorStyles.label);
                            }
                        }

                        EditorGUI.indentLevel -= 1;
                    }
                }
            }
            else
            {
                base.OnInspectorGUI();

                var guiEnabled = GUI.enabled;
                GUI.enabled = false;
                EditorGUILayout.TextField(nameof(NetworkObject.GlobalObjectIdHash), m_NetworkObject.GlobalObjectIdHash.ToString("X"));
                GUI.enabled = guiEnabled;
            }
        }
    }
}
