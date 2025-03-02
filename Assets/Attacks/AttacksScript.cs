using System.Collections.Generic;
using BuildingsFolder;
using BuildingsFolder.BuildingsClasses;
using Cecs.Entity;
using Cecs.Util;
using OlympusDedicatedServer.Components.Buildings;
using OlympusDedicatedServer.Components.WorldComp.Buildings;

namespace Attacks
{
    public class AttacksScript
    {
        /*

            public BuildingsManager BuildingsManager;
            public int Width;
            public int Height;


            public AttacksScript(BuildingsManager buildingsManager)
            {
                BuildingsManager = buildingsManager;
            }

            private void Attack(int targetX, int targetY,AttackType type, uint attacker)
            {
                if (type is AttackType.Dionysos)
                {
                    Building target = BuildingsManager.buildings[(targetX,targetY)];
                    if (target != null && target.OwnerId != attacker)
                    {
                        target.paralyze = true;
                    }
                }
                else if (type is AttackType.Poseidon)
                {
                    foreach (var target in FindBuildOf3Radius(targetX,targetY,worldComponent,attacker))
                    {
                        target.Add(new ParalyzedComponent(PoseidonParalysisTime));
                    }
                }
                else if (type is AttackType.Zeus)
                {
                    var targets = new List<IEntity>();
                    FindBuildInChain(targetX, targetY, worldComponent, attacker, new OpenBitSet(), targets);
                    foreach (var target in FindBuildOf3Radius(targetX,targetY,worldComponent,attacker))
                    {
                        target.Add(new ParalyzedComponent(ZeusParalysisTime));
                    }
                }
                else if (type is AttackType.Athena)
                {
                    var target = worldComponent[targetX, targetY];
                    if (target != null && target.GetComponent<ActionOwnerComponent>() is {} actionOwner &&
                        attacker != actionOwner.Owner)
                    {
                        target.Add(new StolenComponent(AthenaSteelingTime,attacker, target.GetComponent<BuildingComponent>().Owner));
                    }
                }
                else if (type is AttackType.Hades)
                {
                    var target = worldComponent[targetX, targetY];
                    if (target != null && target.GetComponent<ActionOwnerComponent>() is {} actionOwner &&
                        attacker != actionOwner.Owner)
                    {
                        worldComponent[targetX, targetY] = null;
                        target.Destroy();
                    }
                }
            }


            private List<Building> FindBuildOf3Radius(int targetX, int targetY, WorldBuildingsComponent worldComponent, IEntity owner)
            {
                OpenBitSet bitSet = new OpenBitSet();
                List<Building> res = new List<Building>();
                foreach (var (_, x, y) in FindBuildingNeighbor(targetX, targetY))
                {
                    foreach (var (entity2, x2, y2) in FindBuildingNeighbor(x, y))
                    {
                        int index = worldComponent.ToIndex(x2, y2);
                        if (bitSet.Get(index))
                        {
                            continue;
                        }


                        if (entity2 == null || entity2.GetComponent<ActionOwnerComponent>() is not {} actionOwner || owner == actionOwner.Owner)
                        {
                            continue;
                        }

                        bitSet.Set(index);
                        yield return entity2;
                    }
                }
            }

            private void FindBuildInChain(int targetX, int targetY, WorldBuildingsComponent worldComponent,
                IEntity owner,OpenBitSet bitSet,List<IEntity> res)
            {
                IEnumerable<(IEntity?, int, int)> neighbor = worldComponent.FindBuildingNeighbor(targetX, targetY);

                foreach (var (entity,x,y) in neighbor)
                {
                    int index = worldComponent.ToIndex(x, y);
                    if (entity == null || bitSet.Get(index) ||
                        entity.GetComponent<ActionOwnerComponent>() is not {} actionOwner ||
                        owner == actionOwner.Owner)
                    {
                        continue;
                    }
                    bitSet.Set(index);
                    res.Add(entity);
                    FindBuildInChain(x, y, worldComponent, owner, bitSet, res);
                }
            }

            private List<(Building, int, int)> FindBuildingNeighbor(int targetX, int targetY)
            {
                List<(Building, int, int)> res = new List<(Building, int, int)>();
                if (!OutOfGrid(targetX + 1, targetY))
                {
                    res.Add(EntityCoordinates(targetX + 1, targetY));
                }
                if (!OutOfGrid(targetX - 1, targetY))
                {
                    res.Add(EntityCoordinates(targetX - 1, targetY));
                }

                if (targetY % 2 == 1)
                {
                    if (!OutOfGrid(targetX, targetY + 1))
                    {
                        res.Add(EntityCoordinates(targetX, targetY + 1));
                    }
                    if (!OutOfGrid(targetX + 1, targetY + 1))
                    {
                        res.Add(EntityCoordinates(targetX + 1, targetY + 1));
                    }
                    if (!OutOfGrid(targetX, targetY - 1))
                    {
                        res.Add(EntityCoordinates(targetX, targetY - 1));
                    }
                    if (!OutOfGrid(targetX + 1, targetY - 1))
                    {
                        res.Add(EntityCoordinates(targetX + 1, targetY - 1));
                    }
                }
                else
                {
                    if (!OutOfGrid(targetX - 1, targetY + 1))
                    {
                        res.Add(EntityCoordinates(targetX - 1, targetY + 1));
                    }
                    if (!OutOfGrid(targetX, targetY + 1))
                    {
                        res.Add(EntityCoordinates(targetX , targetY + 1));
                    }
                    if (!OutOfGrid(targetX - 1, targetY - 1))
                    {
                        res.Add(EntityCoordinates(targetX - 1, targetY - 1));
                    }
                    if (!OutOfGrid(targetX, targetY - 1))
                    {
                        res.Add(EntityCoordinates(targetX, targetY - 1));
                    }
                }

                return res;
            }
            public bool OutOfGrid(int x, int y)
            {
                return x < 0 || x >= Width || y < 0 || y >= Height;
            }

            private (Building, int, int) EntityCoordinates(int x, int y)
            {
                return (BuildingsManager.buildings[(x,y)], x, y);
            }

            public int ToIndex(int x, int y)
            {
                return (int)(x + y * Width);
            }
        }
        public enum AttackType
        {
            Poseidon,
            Zeus,
            Athena,
            Dionysos,
            Hades
        }*/
    }
}