using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BinaryTreeGenerics
{
    class BinaryTree
    {
        public class BinaryNode
        {
            public BinaryNode left { get; set; }
            public BinaryNode right { get; set; }
            public int value;

            public BinaryNode(int val)
            {
                value = val;
                left = null;
                right = null;
            }
        }

        public BinaryNode root;
        public BinaryTree()
        {
            root = null;
        }

        public BinaryTree(int value)
        {
            root = new BinaryNode(value);
        }

        //non-recursive addition
        public void Add(int value) //node and its meaning
        {
            if (root == null)  //if there is no root
            {
                root = new BinaryNode(value); //add the element as root
                return;
            }

            BinaryNode current = root; //current is equal to root
            bool added = false;
            //we go around the tree
            do
            {
                TreeTraversal(value, ref current, ref added);
            }
            while (!added);
        }

        private static void TreeTraversal(int value, ref BinaryNode current, ref bool added)
        {
            if (value >= current.value)  //let's go right
            {
                if (current.right == null)
                {
                    current.right = new BinaryNode(value);
                    added = true;
                    return;
                }
                else
                {
                    current = current.right;
                    return;
                }

            }
            if (value < current.value) //let's go left
            {
                if (current.left == null)
                {
                    current.left = new BinaryNode(value);
                    added = true;
                    return;
                }
                else
                {
                    current = current.left;
                    return;
                }
            }
            else
            {
                current = current.left;
                return;
            }
        }
    }
}
