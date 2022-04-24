using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class AnimationPath {
    public List<string> pathPoints;
    public List<string> traverseProcessPoints;
    public List<int> weights;
    public AnimationPath() {
        pathPoints = new List<string>();
        traverseProcessPoints = new List<string>();
        weights = new List<int>();
    }
    public AnimationPath(List<string> paths, List<string> traversePath, List<int> w)
    {
        pathPoints = paths;
        traverseProcessPoints = traversePath;
        weights =w;
    }
}
enum AlgorithemType { 
    BFS,
    AStar,
    DFS
}

public class GridCellMonitor : MonoBehaviour
{
    //Get the gridSize from Slide
    [SerializeField]
    private AlgorithemType algorithemType;
    [SerializeField]
    private int gridSize;
    [SerializeField]
    private GameObject alertPanel;
    [SerializeField]
    private int startCellX;
    [SerializeField]
    private int startCellY;
    [SerializeField]
    private GameObject startCell;
    [SerializeField]
    private int endCellX;
    [SerializeField]
    private int endCellY;
    [SerializeField]
    private GameObject endCell;
    [SerializeField]
    private List<string> blockCells;
    [SerializeField]
    private TMP_Text totalStepNumberTxt;
    [SerializeField]
    private GameObject findPathBtn, clearGridBtn, GenerateMapBtn;

    private bool isAnimation;
    private bool isRunAnimation;
    private bool useFullPath;
    private int mapsIndex;

    private List<string> prevPathPoints;
    private List<string> prevTraversePoints;

    // Start is called before the first frame update
    void Start()
    {
        totalStepNumberTxt.text = "";
        algorithemType = AlgorithemType.BFS;
        isAnimation = true;
        isRunAnimation = false;
        startCellX = -1;
        startCellY = -1;
        startCell = null;
        endCellX = -1;
        endCellY = -1;
        endCell = null;
        blockCells = new List<string>();
        blockCells.Clear();
        prevPathPoints = new List<string>();
        prevTraversePoints = new List<string>();
        gridSize = (int)GameObject.Find("Slider").GetComponent<Slider>().value;
        SetGridSize(gridSize);
        useFullPath = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyUp(KeyCode.Escape)) {
            Application.Quit();        
        }
    }

    //when the left mouse botton click on the grid, set the start cell
    public void SetStartCell(int X, int Y, GameObject button) {

        //check the block list, if it exsits, remove from list
        if (blockCells.Contains(X.ToString() + "-" + Y.ToString())) {
            RemoveBlockCells(X.ToString() + "-" + Y.ToString());
        }

        startCellX = X;
        startCellY = Y;
        startCell = button;    
    }

    //when click right mouse button, set the end cell
    public void SetEndCell(int X, int Y, GameObject button)
    {
        //check the block list, if it exsits, remove from list
        if (blockCells.Contains(X.ToString() + "-" + Y.ToString()))
        {
            RemoveBlockCells(X.ToString() + "-"+ Y.ToString());
        }
        endCellX = X;
        endCellY = Y;
        endCell = button;
    }
    //when click mid mouse button, set the block cells
    public void SetBlockCells(string blockStr)
    {
        blockCells.Add(blockStr);
    }


    internal void RemoveBlockCells(string cell)
    {
        if (blockCells.Contains(cell)) {
            //blockCells.
            blockCells.Remove(cell);
        }
    }

    internal void ResetStartCell(int x  , int y)
    {
        startCell = null;
        startCellX = -1;
        startCellY = -1;
    }

    internal void ResetEndCell(int x, int y)
    {
        endCell = null;
        endCellX = -1;
        endCellY = -1;
    }

    internal void RemovePrevStartCell(int X , int Y)
    {
        if (startCellX != -1 && startCellY != -1) {
            //if (X != startCellX && Y != startCellY)
            if (!(X.ToString() + "-" + Y.ToString()).Equals((startCellX.ToString() + "-" +startCellY.ToString())))
            {
                if (startCell.GetComponent<Image>().color == Color.green)
                {
                    startCell.GetComponent<Image>().color = Color.white;
                    startCell.GetComponent<CellControll>().RemoveStartCell();
                }
            }
        }
    }

    internal void RemovePrevEndtCell(int X, int Y)
    {
        if (endCellX != -1 && endCellY != -1)
        {
            //if (X != startCellX && Y != startCellY)
            if (!(X.ToString() + "-" + Y.ToString()).Equals((endCellX.ToString() + "-" + endCellY.ToString())))
            {
                if (endCell.GetComponent<Image>().color == Color.red)
                {
                    endCell.GetComponent<Image>().color = Color.white;
                    endCell.GetComponent<CellControll>().RemoveEndCell();
                }
            }
        }
    }

    public void ClickPathFindingBtn() {
        if(isRunAnimation == false)
        {

            totalStepNumberTxt.text = "";
            ClearPrevPath();
            ClearPreTraversePoints();
            int[,] map = CreateMap();
            int XMax = map.GetLength(0);
            int YMax = map.GetLength(1);

            if (startCellX == -1 && startCellY == -1)
            {
                string str = "NO Start Point";
                //Debug.Log(str);
                DisplayAlertPanel(str);
                //TODO: show message to user, there is no start point
                return;
            }
            else if (endCellX == -1 && endCellY == -1)
            {
                string str = "NO End Point";
                //Debug.Log(str);
                DisplayAlertPanel(str);
                //TODO: show message to user, there is no end point
                return;
            }
            else
            {
                //For create new map data
                
                Debug.Log("StartCell Name:" + startCell.name);
                Debug.Log("EndCell Name:" + endCell.name);
                string blockStr = "";
                foreach (var block in blockCells)
                {
                    blockStr += ("\", \"" + block);
                }
                Debug.Log("Blocks list:" + blockStr);
                
                switch (algorithemType)
                {
                    case AlgorithemType.BFS:
                    case AlgorithemType.DFS:
                        UseBFSAlgorithm(map, XMax, YMax);
                        break;
                    case AlgorithemType.AStar:
                        Point start = new Point(startCellX, startCellY);
                        Point end = new Point(endCellX, endCellY);
                        UseAStarAlgorithm(start, end, map);
                        break;
                    default:
                        break;
                }
            }
        }
    }

    private void UseAStarAlgorithm(Point start, Point end, int[,] map)
    {
        Astar aStar = new Astar(start,end,map);


        if (useFullPath)
        {
            aStar.SetMoveType(1);

        }
        else
        {
            aStar.SetMoveType(0);

        }

        List<string> path = aStar.StartAstarDesc();
        if (path == null) {
            string str = "NO PATH";
            DisplayAlertPanel(str);
            return;
        }
        prevPathPoints = path;

        List<NodeAStar> traverseNodeAStars = aStar.GetTraversNodes();
        List<string> tranverseNodesStr = new List<string>();
        List<int> weights = new List<int>();
        foreach (var item in traverseNodeAStars)
        {
            //Debug.Log(item.Vertex.X + ", " + item.Vertex.Y + " Weight: " + item.Weight);
            tranverseNodesStr.Add(item.Vertex.X.ToString() + "-" + item.Vertex.Y.ToString());
            weights.Add(item.Weight);
        }

        if (isAnimation)
        {
            path.Reverse();
            weights.RemoveAt(tranverseNodesStr.IndexOf(startCell.name));
            tranverseNodesStr.Remove(startCell.name);
            weights.RemoveAt(tranverseNodesStr.IndexOf(endCell.name));
            tranverseNodesStr.Remove(endCell.name);
            prevTraversePoints = tranverseNodesStr;
            AnimationPath animationPath = new AnimationPath(path, tranverseNodesStr, weights);
            StartCoroutine("AnimationGenerateTraverseProcess", animationPath);
        }
        else
        {
            totalStepNumberTxt.text = "Total " + (path.Count - 1).ToString() + " Steps";
            GeneratePath(path);
        }

    }

    private void UseBFSAlgorithm(int[,] map, int XMax, int YMax) {
        //List<Node> nodes = new List<Node>();
        BFS_DFS_Algorithm bFS_dFS_Algorithm = new BFS_DFS_Algorithm();


        if (useFullPath)
        {
            switch (algorithemType)
            {
                case AlgorithemType.BFS:
                    bFS_dFS_Algorithm.nodes = bFS_dFS_Algorithm.TraverseMapFullPath(map, XMax, YMax);
                    break;
                case AlgorithemType.DFS:
                    bFS_dFS_Algorithm.nodes = bFS_dFS_Algorithm.DFSTraverseMapFullPath(map, XMax, YMax);
                    break;
            }
        }
        else
        {
            bFS_dFS_Algorithm.nodes = bFS_dFS_Algorithm.TraverseMapCrossPath(map, XMax, YMax);
        }
        if (algorithemType == AlgorithemType.BFS)
        {
            bFS_dFS_Algorithm.dictionaryStr = bFS_dFS_Algorithm.BFS(bFS_dFS_Algorithm.nodes, new Point(startCellX, startCellY));
        }
        else if (algorithemType == AlgorithemType.DFS)
        {
            bFS_dFS_Algorithm.dictionaryStr = bFS_dFS_Algorithm.DFS(bFS_dFS_Algorithm.nodes, new Point(startCellX, startCellY));
        }
        string endPoint = endCellX.ToString() + "-" + endCellY.ToString();
        if (bFS_dFS_Algorithm.dictionaryStr.ContainsKey(endPoint))
        {
            AnimationPath animationPath = new AnimationPath();
            Dictionary<string, string>.KeyCollection keyCol = bFS_dFS_Algorithm.dictionaryStr.Keys;
            foreach (var item in keyCol)
            {
                animationPath.traverseProcessPoints.Add(item);
            }
            animationPath.traverseProcessPoints.Remove(startCell.name);
            animationPath.traverseProcessPoints.Remove(endCell.name);
            prevTraversePoints = animationPath.traverseProcessPoints;
            while (endPoint != null)
            {
                //Debug.Log(endPoint);
                animationPath.pathPoints.Add(endPoint);
                prevPathPoints.Add(endPoint);
                endPoint = bFS_dFS_Algorithm.dictionaryStr[endPoint];
            }

            if (isAnimation)
            {
                StartCoroutine("AnimationGenerateTraverseProcess", animationPath);
            }
            else
            {
                GeneratePath(animationPath.pathPoints);
                totalStepNumberTxt.text = "Total " + (animationPath.pathPoints.Count-1).ToString() + " Steps";
            }
        }
        else
        {
            string str = "NO PATH";
            DisplayAlertPanel(str);
        }
    }

    private void GeneratePath(List<string> pathPoints)
    {

        for (int i = 1; i < pathPoints.Count-1; i++)        
        {            
            var gameObject = GameObject.Find(pathPoints[i]);
            if (gameObject.GetComponent<CellControll>())
            {
                gameObject.GetComponent<Image>().color = Color.blue;
                //StartCoroutine("PaintPathColor", gameObject);
            }
        }
    }

    private void ClearPrevPath()
    {
        for (int i = 1; i < prevPathPoints.Count - 1; i++)
        {
            var gameObject = GameObject.Find(prevPathPoints[i]);
            if (gameObject.GetComponent<CellControll>())
            {
                if(gameObject.GetComponent<Image>().color == Color.blue)
                    gameObject.GetComponent<Image>().color = Color.white;
            }
        }
        prevPathPoints.Clear();
    }

    private void ClearPreTraversePoints() {
        for (int i = 0; i < prevTraversePoints.Count; i++)
        {
            var gameObject = GameObject.Find(prevTraversePoints[i]);
            if (gameObject.GetComponent<CellControll>())
            {
                if (gameObject.GetComponent<Image>().color == Color.yellow)
                {
                    gameObject.GetComponent<Image>().color = Color.white;
                }
                gameObject.GetComponentInChildren<Text>().text = "";
            }
        }
        prevTraversePoints.Clear();
    }

    IEnumerator AnimationGeneratePath(List<string> pathPoints) {
        isRunAnimation = true;
        findPathBtn.GetComponent<Button>().interactable = false;
        clearGridBtn.GetComponent<Button>().interactable = false;
        GenerateMapBtn.GetComponent<Button>().interactable = false;
        GameObject.Find("Slider").GetComponent<Slider>().interactable = false;
        for (int i = pathPoints.Count - 2; i >= 1; i--)
        {
            var gameObject = GameObject.Find(pathPoints[i]);
            if (gameObject.GetComponent<CellControll>())
            {
                gameObject.GetComponent<Image>().color = Color.blue;
                yield return new WaitForSeconds(0.05f); 
                //StartCoroutine("PaintPathColor", gameObject);
            }
        }
        totalStepNumberTxt.text = "Total " + (pathPoints.Count-1).ToString() + " Steps";
        isRunAnimation = false;
        findPathBtn.GetComponent<Button>().interactable = true;
        clearGridBtn.GetComponent<Button>().interactable = true;
        GenerateMapBtn.GetComponent<Button>().interactable = true;
        GameObject.Find("Slider").GetComponent<Slider>().interactable = true;

    }

    IEnumerator AnimationGenerateTraverseProcess(AnimationPath animationPath)
    {
        isRunAnimation = true;
        findPathBtn.GetComponent<Button>().interactable = false;
        clearGridBtn.GetComponent<Button>().interactable = false;
        GenerateMapBtn.GetComponent<Button>().interactable = false;
        GameObject.Find("Slider").GetComponent<Slider>().interactable = false;
        bool jumpWeight = true;
        if (animationPath.weights.Count != 0) {
            jumpWeight = false;
        }

        for (int i = 0; i < animationPath.traverseProcessPoints.Count; i++)
        {
            var gameObject = GameObject.Find(animationPath.traverseProcessPoints[i]);
            if (gameObject.GetComponent<CellControll>())
            {
                gameObject.GetComponent<Image>().color = Color.yellow;
                if (!jumpWeight)
                {
                    gameObject.GetComponentInChildren<Text>().text = animationPath.weights[i].ToString();
                }
                //yield return new WaitForSeconds(0.10f);
                yield return new WaitForSeconds(0.01f);
                //StartCoroutine("PaintPathColor", gameObject);
            }
        }
        isRunAnimation = false;
        findPathBtn.GetComponent<Button>().interactable = true;
        clearGridBtn.GetComponent<Button>().interactable = true;
        GenerateMapBtn.GetComponent<Button>().interactable = true;
        GameObject.Find("Slider").GetComponent<Slider>().interactable = true;
        StartCoroutine("AnimationGeneratePath", animationPath.pathPoints);
    }

    private int[,] CreateMap()
    {
        //int[,] tempMap = {
        //        {0,0,0,0,0,0,0,0,0,0},
        //        {0,0,0,0,0,0,0,0,0,0},
        //        {0,0,0,0,0,0,0,0,0,0},
        //        {0,0,0,0,0,0,0,0,0,0},
        //        {0,0,0,0,0,0,0,0,0,0},
        //        {0,0,0,0,0,0,0,0,0,0},
        //        {0,0,0,0,0,0,0,0,0,0},
        //        {0,0,0,0,0,0,0,0,0,0},
        //        {0,0,0,0,0,0,0,0,0,0},
        //        {0,0,0,0,0,0,0,0,0,0}
        //    };
        //return tempMap;

        int[,] tempMap = new int[gridSize, gridSize];
        for (int i = 0; i < gridSize; i++)
        {
            for (int j = 0; j < gridSize; j++)
            {
                tempMap[i, j] = 0;
            }

        }

        if (blockCells.Count != 0) {
            for (int i = 0; i < blockCells.Count; i++)
            {
                int XPositon =int.Parse(blockCells[i].Split('-')[0]);
                int YPosition = int.Parse(blockCells[i].Split('-')[1]);
                tempMap[XPositon, YPosition] = 1;
            }
        }

        return tempMap;

    }

    //get the grid size value from the slider
    public void SetGridSize(float value) {
        //Debug.Log(value);
        ClearGrid();
        ResetGrid();
        gridSize = (int)value;
        for (int i = gridSize; i < 20; i++)
        {
            for (int j = 0; j < 20; j++)
            {
                GameObject.Find(i.ToString() + "-" + j.ToString()).GetComponent<Button>().interactable = false;
                GameObject.Find(j.ToString() + "-" + i.ToString()).GetComponent<Button>().interactable = false;
            }
        }

        for (int i = 0; i < gridSize; i++)
        {
            for (int j = 0; j < gridSize; j++)
            {
                GameObject.Find(i.ToString() + "-" + j.ToString()).GetComponent<Button>().interactable = true;
            }
        }
    }

    public void ClearGrid() {
        ResetGrid();
        GameObject[] gameObjects = GameObject.FindGameObjectsWithTag("Cell");
        if (gameObjects.Length != 0) {
            foreach (GameObject cell in gameObjects)
            {
                cell.GetComponent<CellControll>().Reset();
                cell.GetComponentInChildren<Text>().text = "";
            }    
        }
    }

    private void ResetGrid() {
        startCellX = -1;
        startCellY = -1;
        startCell = null;
        endCellX = -1;
        endCellY = -1;
        endCell = null;
        blockCells.Clear();
    }

    public void IsAnimaiton() {
        isAnimation = !isAnimation;
    }

    private void DisplayAlertPanel(string str) {
        alertPanel.transform.GetChild(0).GetComponent<TMP_Text> ().text = str;
        alertPanel.SetActive(true);
    }

    public void GetPointType(int value) {
        //int a = GameObject.Find("Dropdown").GetComponent<TMP_Dropdown>().value;
        Debug.Log(value);
        if (value == 0)
        {
            useFullPath = true;
        }
        else if (value == 1) {
            useFullPath = false;
        }
    }

    public void BFSToggleOnClick(bool isOn) {
        algorithemType = AlgorithemType.BFS;
    }

    public void AStarToggleOnClick(bool isOn) {
        algorithemType = AlgorithemType.AStar;
    }

    public void DFSToggleOnClick(bool isOn)
    {
        algorithemType = AlgorithemType.DFS;
    }

    public void GenarateMap() {
        mapsIndex++;
        if (mapsIndex > 11) {
            mapsIndex = 0;
        }
        switch (mapsIndex)
        {
            case 0:
                GetValueFromStaticMap(CaseMap_2.GridSize, CaseMap_2.StartPointX, CaseMap_2.StartPointY,
                        CaseMap_2.EndPointX, CaseMap_2.EndPointY, CaseMap_2.StartPoint, CaseMap_2.EndPoint, CaseMap_2.Blocks);
                break;
            case 1:
                GetValueFromStaticMap(CaseMap_1.GridSize, CaseMap_1.StartPointX, CaseMap_1.StartPointY,
                    CaseMap_1.EndPointX,CaseMap_1.EndPointY,CaseMap_1.StartPoint,CaseMap_1.EndPoint,CaseMap_1.Blocks);
                break;
            case 2:
                GetValueFromStaticMap(CaseMap_3.GridSize, CaseMap_3.StartPointX, CaseMap_3.StartPointY,
                    CaseMap_3.EndPointX, CaseMap_3.EndPointY, CaseMap_3.StartPoint, CaseMap_3.EndPoint, CaseMap_3.Blocks);
                break;
            case 3:
                GetValueFromStaticMap(CaseMap_4.GridSize, CaseMap_4.StartPointX, CaseMap_4.StartPointY,
                    CaseMap_4.EndPointX, CaseMap_4.EndPointY, CaseMap_4.StartPoint, CaseMap_4.EndPoint, CaseMap_4.Blocks);
                break;
            case 4:
                GetValueFromStaticMap(CaseMap_5.GridSize, CaseMap_5.StartPointX, CaseMap_5.StartPointY,
                    CaseMap_5.EndPointX, CaseMap_5.EndPointY, CaseMap_5.StartPoint, CaseMap_5.EndPoint, CaseMap_5.Blocks);
                break;
            case 5:
                GetValueFromStaticMap(CaseMap_6.GridSize, CaseMap_6.StartPointX, CaseMap_6.StartPointY,
                    CaseMap_6.EndPointX, CaseMap_6.EndPointY, CaseMap_6.StartPoint, CaseMap_6.EndPoint, CaseMap_6.Blocks);
                break;
            case 6:
                GetValueFromStaticMap(CaseMap_7.GridSize, CaseMap_7.StartPointX, CaseMap_7.StartPointY,
                    CaseMap_7.EndPointX, CaseMap_7.EndPointY, CaseMap_7.StartPoint, CaseMap_7.EndPoint, CaseMap_7.Blocks);
                break;
            case 7:
                GetValueFromStaticMap(CaseMap_8.GridSize, CaseMap_8.StartPointX, CaseMap_8.StartPointY,
                    CaseMap_8.EndPointX, CaseMap_8.EndPointY, CaseMap_8.StartPoint, CaseMap_8.EndPoint, CaseMap_8.Blocks);
                break;
            case 8:
                GetValueFromStaticMap(CaseMap_9.GridSize, CaseMap_9.StartPointX, CaseMap_9.StartPointY,
                    CaseMap_9.EndPointX, CaseMap_9.EndPointY, CaseMap_9.StartPoint, CaseMap_9.EndPoint, CaseMap_9.Blocks);
                break;
            case 9:
                GetValueFromStaticMap(CaseMap_10.GridSize, CaseMap_10.StartPointX, CaseMap_10.StartPointY,
                    CaseMap_10.EndPointX, CaseMap_10.EndPointY, CaseMap_10.StartPoint, CaseMap_10.EndPoint, CaseMap_10.Blocks);
                break;
            case 10:
                GetValueFromStaticMap(CaseMap_11.GridSize, CaseMap_11.StartPointX, CaseMap_11.StartPointY,
                    CaseMap_11.EndPointX, CaseMap_11.EndPointY, CaseMap_11.StartPoint, CaseMap_11.EndPoint, CaseMap_11.Blocks);
                break;
            case 11:
                GetValueFromStaticMap(CaseMap_12.GridSize, CaseMap_12.StartPointX, CaseMap_12.StartPointY,
                    CaseMap_12.EndPointX, CaseMap_12.EndPointY, CaseMap_12.StartPoint, CaseMap_12.EndPoint, CaseMap_12.Blocks);
                break;
        }

        startCell.GetComponent<CellControll>().SetStartPoint();
        endCell.GetComponent<CellControll>().SetEndPoint();
    }

    private void GetValueFromStaticMap(int gz, int sx, int sy, int ex, int ey, string sp, string ep,string[] strs) {

        gridSize = gz;
        GameObject.Find("GridSlider").GetComponentInChildren<Slider>().value = gz;
        SetGridSize(gz);
        startCellX = sx;
        startCellY = sy;
        endCellX = ex;
        endCellY = ey;
        startCell = GameObject.Find(sp);
        endCell = GameObject.Find(ep);
        foreach (var item in strs)
        {
            blockCells.Add(item);
            GameObject.Find(item).GetComponent<CellControll>().SetBlockPoint();
        }

    }
}
