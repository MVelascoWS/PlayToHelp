using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
using UnityEngine.Events;
using TMPro;

[RequireComponent(typeof(ARRaycastManager))]
public class PlaceObjectsOnPlane : MonoBehaviour
{
    [SerializeField]
    [Tooltip("Instantiates this prefab on a plane at the touch location.")]
    GameObject m_PlacedPrefab;
    public TextMeshProUGUI textDebug;
    public UnityEvent OnPrefabPlace;
    private RaycastHit hit;
    /// <summary>
    /// The prefab to instantiate on touch.
    /// </summary>
    public GameObject placedPrefab
    {
        get { return m_PlacedPrefab; }
        set { m_PlacedPrefab = value; }
    }

    /// <summary>
    /// The object instantiated as a result of a successful raycast intersection with a plane.
    /// </summary>
    public GameObject spawnedObject { get; private set; }

    /// <summary>
    /// Invoked whenever an object is placed in on a plane.
    /// </summary>
    public static event Action onPlacedObject;

    ARRaycastManager m_RaycastManager;

    static List<ARRaycastHit> s_Hits = new List<ARRaycastHit>();
    
    [SerializeField]
    int m_MaxNumberOfObjectsToPlace = 1;

    int m_NumberOfPlacedObjects = 0;

    [SerializeField]
    bool m_CanReposition = true;

    public bool canReposition
    {
        get => m_CanReposition;
        set => m_CanReposition = value;
    }

    void Awake()
    {
        m_RaycastManager = GetComponent<ARRaycastManager>();
    }

    void Update()
    {
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);

            if (touch.phase == TouchPhase.Began)
            {
                if (Physics.Raycast(Camera.main.ScreenPointToRay(touch.position), out hit))
                {
                    if (hit.collider.transform.CompareTag("Road"))
                    {
                        
                        
                        return;
                    }
                    
                    
                }
                if (m_RaycastManager.Raycast(touch.position, s_Hits, TrackableType.PlaneWithinPolygon))
                {
                    Pose hitPose = s_Hits[0].pose;
                    
                    if (m_NumberOfPlacedObjects < m_MaxNumberOfObjectsToPlace)
                    {
                        spawnedObject = Instantiate(m_PlacedPrefab, hitPose.position, hitPose.rotation);
                        OnPrefabPlace.Invoke();
                        m_NumberOfPlacedObjects++;
                    }
                    else
                    {
                        return;
                        if (m_CanReposition)
                        {
                            spawnedObject.transform.SetPositionAndRotation(hitPose.position, hitPose.rotation);
                        }
                    }
                    
                    if (onPlacedObject != null)
                    {
                        onPlacedObject();
                    }
                }
            }
        }
    }
}
