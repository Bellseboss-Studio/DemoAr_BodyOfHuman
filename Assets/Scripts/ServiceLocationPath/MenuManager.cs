namespace ServiceLocatorPath
{
    public  class MenuManager : IMenuManager
    {
        private int _indexInMenu;
        
        public void SetIndexDefault(int index)
        {
            _indexInMenu = index;
        }

        public int GetIndex()
        {
            return _indexInMenu;
        }
    }
}