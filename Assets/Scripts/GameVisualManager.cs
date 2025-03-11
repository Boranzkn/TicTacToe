using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class GameVisualManager : NetworkBehaviour
{
    private const float GRID_SIZE = 3.1f;

    [SerializeField] private Transform crossPrefab;
    [SerializeField] private Transform circlePrefab;
    [SerializeField] private Transform lineCompletePrefab;

    private List<GameObject> visualGameObjectList;

    private void Awake()
    {
        visualGameObjectList = new List<GameObject>();
    }

    private void Start()
    {
        GameManager.Instance.OnClickedOnGridPosition += GameManager_OnClickedOnGridPosition;
        GameManager.Instance.OnGameWin += GameManager_OnGameWin;
        GameManager.Instance.OnRematch += GameManager_OnRematch;
    }

    private void GameManager_OnRematch(object sender, System.EventArgs e)
    {
        if (!NetworkManager.Singleton.IsServer)
        {
            return;
        }

        // Destroy all visuals for rematch.
        foreach (GameObject visualGameObject in visualGameObjectList)
        {
            Destroy(visualGameObject);
        }
        visualGameObjectList.Clear();
    }

    private void GameManager_OnGameWin(object sender, GameManager.OnGameWinEventArgs e)
    {
        if (!NetworkManager.Singleton.IsServer)
        {
            return;
        }

        int eulerZ = 0;
        switch (e.line.orientation)
        {
            case GameManager.Orientation.Horizontal: eulerZ = 0; break;
            case GameManager.Orientation.Vertical: eulerZ = 90; break;
            case GameManager.Orientation.DiagonalA: eulerZ = 45; break;
            case GameManager.Orientation.DiagonalB: eulerZ = -45; break;
        }

        Transform lineCompleteTransform = Instantiate(lineCompletePrefab, GetGridWorldPosition(e.line.centerGridPosition.x, e.line.centerGridPosition.y), Quaternion.Euler(0,0,eulerZ));
        lineCompleteTransform.GetComponent<NetworkObject>().Spawn(true);
        visualGameObjectList.Add(lineCompleteTransform.gameObject);
    }

    private void GameManager_OnClickedOnGridPosition(object sender, GameManager.OnClickedOnGridPositionEventArgs e)
    {
        SpawnObjectRpc(e.x, e.y, e.playerType);
    }

    [Rpc(SendTo.Server)]
    private void SpawnObjectRpc(int x, int y, GameManager.PlayerType playerType)
    {
        Transform prefab;
        switch (playerType)
        {
            default:
            case GameManager.PlayerType.Circle:
                prefab = circlePrefab;
                break;
            case GameManager.PlayerType.Cross:
                prefab = crossPrefab;
                break;
        }

        Transform spawnedCrossTransform = Instantiate(prefab, GetGridWorldPosition(x, y), Quaternion.identity);
        spawnedCrossTransform.GetComponent<NetworkObject>().Spawn(true);
        visualGameObjectList.Add(spawnedCrossTransform.gameObject);
    }

    private Vector2 GetGridWorldPosition(int x, int y)
    {
        return new Vector2(-GRID_SIZE + x * GRID_SIZE, -GRID_SIZE + y * GRID_SIZE);
    }
}
