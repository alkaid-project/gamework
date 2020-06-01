namespace core
{
    public interface IMapGen
    {
        //表示生成的顺序
        int GetGenType();

        void GenMap(Map map, int x, int y);
    }
    
}