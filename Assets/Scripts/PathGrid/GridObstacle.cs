using System.Collections;
using Pathfinding;
using UnityEngine;
using UnityEngine.Tilemaps;

public class GridObstacle : MonoBehaviour
{
    public static GridGraph gridGraph;

    Coroutine calculatingBounds;
    void Awake(){
        gridGraph ??= AstarPath.active.data.gridGraph;
        calculatingBounds = StartCoroutine(RecalculateGraphInBounds());
    }

    private void OnEnable() {
        if(calculatingBounds == null)
            calculatingBounds = StartCoroutine(RecalculateGraphInBounds());
    }

    IEnumerator RecalculateGraphInBounds()
    {
        yield return new WaitForSeconds(1f);
        Bounds bounds = GetComponent<CompositeCollider2D>().bounds;
        GraphUpdateObject guo = new GraphUpdateObject(bounds);
        AstarPath.active.UpdateGraphs(guo);
    }
}
