namespace GGame.Hybird
{
    public  interface Frame
    {

        void OnShow(System.Object o);


        void OnHide();

        void OnInit();

        void OnDestroy();
    }
}