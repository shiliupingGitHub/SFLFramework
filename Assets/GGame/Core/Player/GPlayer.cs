using System;
using System.Collections.Generic;

namespace GGame.Core
{
    public class GPlayer : IDisposable
    {
        public World World { get; set; }
        public ulong Id { get; set; }
        public List<int> Cards { get; set; }= new List<int>();
        public Entity ExploreEntity { get; set; }
        public List<Entity> FightEntitys { get; set; } = new List<Entity>();
        public void Dispose()
        {
            Id = 0;
            Cards.Clear();

            if (null != ExploreEntity)
            {
                World.RemoveEntity(ExploreEntity.Id);
            }

            foreach (var entity in FightEntitys)
            {
                World.RemoveEntity(entity.Id);
            }
            FightEntitys.Clear();
            ExploreEntity = null;
            ObjectServer.Instance.Recycle(this);
        }
    }
}