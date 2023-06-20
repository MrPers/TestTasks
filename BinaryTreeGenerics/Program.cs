using System;
using System.Collections;
using System.Collections.Generic;
using static BinaryTreeGenerics.BinaryTree;

namespace BinaryTreeGenerics
{
    class Program
    {
        static void Main()
        {
            BinaryTree numbers = new BinaryTree();
            numbers.Add(30);
            //numbers.Add(45);
            numbers.Add(15);
            numbers.Add(25);
            numbers.Add(35);
            numbers.Add(5);
            numbers.Add(20);
            numbers.Add(3);
            numbers.Add(103);

        }
    }
}

