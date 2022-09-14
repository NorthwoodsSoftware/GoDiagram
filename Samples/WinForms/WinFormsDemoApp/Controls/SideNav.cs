/* Copyright 1998-2022 by Northwoods Software Corporation. */

using System;
using System.ComponentModel;
using System.Windows.Forms;

namespace WinFormsDemoApp {
  /// <summary>
  /// No special behavior yet.
  /// </summary>
  [ToolboxItem(true)]
  public partial class SideNav : ListBox {

    public SideNav() {
      InitializeComponent();
    }

    public SideNav(IContainer container) {
      container.Add(this);

      InitializeComponent();
    }
  }

  public class NavItem {
    public NavItem(string name, Type type) {
      Name = name;
      ControlType = type;
    }
    public string Name { get; }
    public Type ControlType { get; }

    public override string ToString() => Name;
  }
}
