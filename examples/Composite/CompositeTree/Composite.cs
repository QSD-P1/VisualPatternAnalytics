using System;
using System.Collections;
using System.Collections.Generic;

namespace CompositeTree
{
    class Composite : Component
    {
        List<Component> nodes = new List<Component>();
        public Composite(string name) :
            base(name)
        {
        }

        public override void Operation()
        {
            Console.WriteLine(name);
            foreach (Component item in nodes)
                item.Operation();
        }
        public override void Add(Component component)
        {
            nodes.Add(component);
        }
        public override void Remove(Component component)
        {
            nodes.Remove(component);
        }
        public override Component GerChild(int index)
        {
            return nodes[index] as Component;
        }
    }
}
