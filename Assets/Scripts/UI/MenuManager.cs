using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;

public class MenuManager : MonoBehaviour
{
    public static MenuManager instance;

    [SerializeField] private Menu[] _menus;

    private void Awake()
    {
        instance = this;
    }
    public void OpenMenu(string menuName)
    {
        foreach (Menu menu in _menus)
        {
            if (menu.menuName == menuName)
            {
                menu.Open();
            }
            else
            {
                menu.Close();
            }
        }
    }
}
