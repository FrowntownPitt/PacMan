using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Maze : MonoBehaviour {
    
    public IntVector2 size;

    public float delayTime;

    public MazeCell cellPrefab;

    private MazeCell[,] cells;

    public MazePassage passagePrefab;
    public MazeWall wallPrefab;


    public IntVector2 RandomCoordinates
    {
        get
        {
            return new IntVector2(Random.Range(0, size.x), Random.Range(0, size.z));
        }
    }
    
    public IEnumerator Generate()
    {
        cells = new MazeCell[size.x, size.z];

        WaitForSeconds delay = new WaitForSeconds(delayTime);


        List<MazeCell> activeCells = new List<MazeCell>();
        DoFirstGenerationStep(activeCells);

        IntVector2 coordinates = RandomCoordinates;
        while (activeCells.Count > 0)
        {
            yield return delay;
            DoNextGenerationStep(activeCells);
            //CreateCell(coordinates);
            //coordinates += MazeDirections.RandomValue.ToIntVector2();
        }

        Debug.Log("Finished");
    }

    private void DoFirstGenerationStep(List<MazeCell> activeCells)
    {
        activeCells.Add(CreateCell(RandomCoordinates));
    }

    private void DoNextGenerationStep(List<MazeCell> activeCells)
    {
        int index = activeCells.Count - 1;
        MazeCell cell = activeCells[index];
        MazeDirection direction = MazeDirections.RandomValue;

        IntVector2 coordinates = cell.coordinates + direction.ToIntVector2();

        if(ContainsCoordinates(coordinates) && GetCell(coordinates) == null)
        {
            activeCells.Add(CreateCell(coordinates));
        }
        else 
        {
            activeCells.RemoveAt(index);
        }
    }

    private MazeCell GetCell(IntVector2 coordinates)
    {
        return cells[coordinates.x, coordinates.z];
    }

    public bool ContainsCoordinates(IntVector2 coordinates)
    {
        return coordinates.x >= 0 && coordinates.x < size.x && coordinates.z >= 0 && coordinates.z < size.z;
    }

    public MazeCell CreateCell(IntVector2 coordinates)
    {
        MazeCell cell = Instantiate(cellPrefab) as MazeCell;

        cells[coordinates.x, coordinates.z] = cell;
        cell.name = "(" + coordinates.x + "," + coordinates.z + ")";
        cell.coordinates = coordinates;
        cell.transform.parent = transform;
        cell.transform.localPosition = new Vector3(coordinates.x - (size.x - 1) * 0.5f, 0, coordinates.z - (size.z - 1) * 0.5f);

        return cell;
    }
}
