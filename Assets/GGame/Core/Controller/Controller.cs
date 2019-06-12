using System;

namespace GGame.Core
{
    public class Controller
    {
        public Action<Entity,Entity, HurtData> OnHurt;
    }
}