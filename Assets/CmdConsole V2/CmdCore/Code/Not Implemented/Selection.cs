using System.Collections.Generic;

namespace CmdConsole
{
    public class Selection<T>
    {
        Selection(List<T> list)
        {
            SelectionList = list;
        }
        public List<T> SelectionList;
        private int index = 0;

        public T GetCurrent()
        {
            return SelectionList[index];
        }

        public T Increment()
        {
            if (index < SelectionList.Count) index++;
            return SelectionList[index];
        }
        public T Decrement()
        {
            if (index > 0) index--;
            return SelectionList[index];
        }
    }
}
