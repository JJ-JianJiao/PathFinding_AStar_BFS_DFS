using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;


public class CellControll : MonoBehaviour, IPointerClickHandler
{
    [SerializeField]
    private GameObject cellGrid;
    private GridCellMonitor gridCellMonitor;

    [SerializeField]
    private int positonX;
    [SerializeField]
    private string positonXStr;
    [SerializeField]
    private int positonY;
    [SerializeField]
    private string positonYStr;
    [SerializeField]
    private bool startCell;
    [SerializeField]
    private bool BlockCell;
    [SerializeField]
    private bool EndCell;
    private Image image;

    private void Awake()
    {
        image = GetComponent<Image>();
        cellGrid = GameObject.Find("CellGrid");
        if (cellGrid)
            gridCellMonitor = cellGrid.GetComponent<GridCellMonitor>();
    }

    private void Start()
    {
        //this part is for the name which are pure number, like 99 => x = 9, y = 9
        /*
         * 
        int number;
        if (Int32.TryParse(gameObject.name,out number)) {
            positonX = number / 10;
            positonY = number % 10;
        }
        */

        string name = gameObject.name;
        string[] splitNames = name.Split('-');
        positonXStr = splitNames[0];
        positonYStr = splitNames[1];

        positonX = int.Parse(positonXStr);
        positonY = int.Parse(positonYStr);

        startCell = false;
        BlockCell = false;
        EndCell = false;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (gameObject.GetComponent<Button>().interactable)
        {
            if (eventData.button == PointerEventData.InputButton.Left)
            {
                //Debug.Log("Left Click");
                if (!startCell)
                {
                    //check the previous cell, if cell is start cell, reset.
                    gridCellMonitor.RemovePrevStartCell(positonX, positonY);

                    gridCellMonitor.SetStartCell(positonX, positonY, gameObject);
                    image.color = Color.green;
                    gameObject.GetComponentInChildren<Text>().text = "";
                    startCell = true;
                }
                else
                {
                    gridCellMonitor.ResetStartCell(positonX, positonY);
                    image.color = Color.white;
                    gameObject.GetComponentInChildren<Text>().text = "";
                    startCell = false;
                }

                if (BlockCell)
                {
                    BlockCell = false;


                }
                if (EndCell)
                {
                    gridCellMonitor.ResetEndCell(positonX,positonY);
                    EndCell = false;
                }
            }
            else if (eventData.button == PointerEventData.InputButton.Middle)
            {
                //Debug.Log("Middle Click");
                if (!BlockCell)
                {
                    image.color = Color.black;
                    BlockCell = true;
                    gameObject.GetComponentInChildren<Text>().text = "";
                    gridCellMonitor.SetBlockCells(positonX.ToString() + "-" + positonY.ToString());

                }
                else
                {
                    gameObject.GetComponentInChildren<Text>().text = "";
                    gridCellMonitor.RemoveBlockCells(positonX.ToString() + "-" + positonY.ToString());
                    image.color = Color.white;
                    BlockCell = false;
                }
                if (startCell)
                {
                    startCell = false;
                    //gridCellMonitor.set
                    gridCellMonitor.ResetStartCell(positonX, positonY);
                }
                if (EndCell)
                {
                    EndCell = false;
                    gridCellMonitor.ResetEndCell(positonX, positonY);
                }
            }
            else if (eventData.button == PointerEventData.InputButton.Right)
            {
                //Debug.Log("Right Click");

                if (!EndCell)
                {
                    gridCellMonitor.RemovePrevEndtCell(positonX, positonY);
                    gridCellMonitor.SetEndCell(positonX, positonY, gameObject);
                    image.color = Color.red;
                    gameObject.GetComponentInChildren<Text>().text = "";
                    EndCell = true;
                }
                else
                {
                    gridCellMonitor.ResetEndCell(positonX, positonY);
                    image.color = Color.white;
                    gameObject.GetComponentInChildren<Text>().text = "";
                    EndCell = false;
                    //gridCellMonitor.RemoveEndCell();
                }
                if (startCell)
                {
                    startCell = false;
                    gridCellMonitor.ResetStartCell(positonX, positonY);

                }
                if (BlockCell)
                {
                    BlockCell = false;
                }
            }
        }
    }

    internal void RemoveStartCell()
    {
        startCell = false; 
    }
    internal void RemoveEndCell()
    {
        EndCell = false;
    }

    internal void Reset()
    {
        startCell = false;
        BlockCell = false;
        EndCell = false;
        image.color = Color.white;
    }

    public void SetStartPoint() {
        image.color = Color.green;
        startCell = true;
        EndCell = false;
        BlockCell = false;
    }

    public void SetEndPoint()
    {
        image.color = Color.red;
        startCell = false;
        EndCell = true;
        BlockCell = false;
    }

    public void SetBlockPoint()
    {
        image.color = Color.black;
        startCell = false;
        EndCell = false;
        BlockCell = true;
    }
}
