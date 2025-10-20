using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class DestructableObjectController : MonoBehaviour
{
    public GameObject[] roots = new GameObject[4];
    [HideInInspector] public DestroyedPieceController[] root_dest_pieces = new DestroyedPieceController[4];
    public List<DestroyedPieceController> destroyed_pieces = new List<DestroyedPieceController>();

    private void Awake()
    {
        for (int _i = 0; _i < 4; _i++)
        {
            root_dest_pieces[_i] = roots[_i].GetComponent<DestroyedPieceController>();
        }
        Debug.Log("destroyed_pieces count: " + destroyed_pieces.Count);
    }

    [ContextMenu("Make Editor")]
    public void MakeEditor()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            var child = transform.GetChild(i);
            var _dpc = child.gameObject.AddComponent<DestroyedPieceController>();
            
            var _rigidbody = child.gameObject.AddComponent<Rigidbody>();
            _rigidbody.isKinematic = false;
            _rigidbody.useGravity = false;

            var _mc = child.gameObject.AddComponent<MeshCollider>();
            _mc.convex = true;
            
            if(destroyed_pieces.Contains(_dpc) == false)
                destroyed_pieces.Add(_dpc);
        }
    }
    
    [ContextMenu("Update List Of Destroyed Pieces")]
    public void UpdateListOfDestroyedPieces()
    {
        destroyed_pieces.Clear();
        for (int i = 0; i < transform.childCount; i++)
        {
            var child = transform.GetChild(i);
            var _dpc = child.gameObject.GetComponent<DestroyedPieceController>();
            if(_dpc)
                destroyed_pieces.Add(_dpc);
        }
#if UNITY_EDITOR
        UnityEditor.EditorUtility.SetDirty(this);
#endif
    }
    
    private IEnumerator Start()
    {
        yield return new WaitForEndOfFrame();
        foreach (var piece in destroyed_pieces)
        {
            piece.MakeStatic();
        }
    }

    private void Update()
    {
        if (DestroyedPieceController.is_dirty)
        {
            foreach (var destroyed_piece in destroyed_pieces)
            {
                destroyed_piece.visited = false;
            }

            // do a breadth first search to find all connected pieces
            for (int i = 0; i < 4; i++)
                FindAllConnectedPieces(root_dest_pieces[i]);

            // drop all pieces not reachable from root
            foreach (var piece in destroyed_pieces)
            {
                if (piece && !piece.visited)
                {
                    piece.Drop();
                }
            }
        }
    }

    private void FindAllConnectedPieces(DestroyedPieceController destroyed_piece)
    {
        if (destroyed_piece.visited)
            return;
        if (!destroyed_piece.is_connected)
            return;

        destroyed_piece.visited = true;
        foreach (var _pdc in destroyed_piece.connected_to)
        {
            FindAllConnectedPieces(_pdc);
        }
    } 
}