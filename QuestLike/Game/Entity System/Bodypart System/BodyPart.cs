using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QuestLike.Combat;
using Microsoft.Xna.Framework;
using QuestLike.Effects;
using Newtonsoft.Json;

namespace QuestLike.Combat
{
    public enum DamageLevel
    {
        Perfect,
        Fine,
        Bruised,
        Horrible,
        Failing,
        Destroyed
    }
}

namespace QuestLike.Organs
{
    [JsonObject(MemberSerialization = MemberSerialization.OptOut)]
    public class BloodData
    {
        public float bloodPressure = 1;
        public float bloodPressureFalloff = 0.025f;
        public float oxygenatedBlood;
        public float deoxygenatedBlood;
        public float idealBloodCapacity;
        public float hyperBloodLevel = 35;
        public float hypoBloodLevel = 10;
        public float oxygenUse = 0.5f;
    }

    [JsonObject(MemberSerialization = MemberSerialization.OptIn)]
    public class BodyPart : Item, IDamagable, IEffectable, IEquipable
    {
        [JsonProperty]
        protected Damager damager = new Damager();
        [JsonProperty]
        protected Effector effector;
        [JsonProperty]
        protected EquipmentManager equipmentManager;

        [JsonProperty(IsReference = true)]
        public BodyPart parent;
        [JsonIgnore]
        public BodyPart Parent { get => parent; set => parent = value; }

        [JsonProperty]
        public BloodData bloodData = new BloodData();
        [JsonProperty]
        public bool usesBlood = true;

        [JsonProperty]
        protected float damage = 0;
        [JsonIgnore]
        public float Damage
        {
            get
            {
                return damager.Damage;
            }
        }

        [JsonIgnore]
        public float BloodPressure
        {
            get
            {
                return bloodData.bloodPressure;
            }
        }

        public BodyPart() :base()
        {

        }

        public BodyPart(string name, string[] ids) : this(name, "", "", ids)
        {
        }

        public BodyPart(string name, string shortdesc, string desc, string[] ids) : base(name, shortdesc, desc, ids)
        {
            effector = new Effector(this);
            equipmentManager = new EquipmentManager(this);
            AddCollection<BodyPart>();
            AddCollection<BloodVessel>();
            AddCollection<Nerve>();
            grabable = false;
        }

        public BodyPart(string name, string desc, string[] ids) : this(name, "", desc, ids)
        {
        }

        [JsonIgnore]
        protected string specifitcationString
        {
            get
            {

                string text = "";
                text += "\n\nTotal blood".Pad(29) + TotalBlood.ToString("0.0") + "ml" + ((bloodData.oxygenatedBlood > bloodData.hyperBloodLevel) ? $" <{Color.Orange.ToInteger()},>(hypertense)@" : "");
                text += "\nOxygenated blood".Pad(28) + bloodData.oxygenatedBlood.ToString("0.0") + "ml";
                text += "\nDeoxygenated blood".Pad(28) + bloodData.deoxygenatedBlood.ToString("0.0") + "ml";
                text += "\nBlood pressure".Pad(28) + BloodPressureState(bloodData.bloodPressure);
                text += "\nHealth".Pad(28) + DamageLevel.ToString();
                text += "\nConnected to nervous system".Pad(28) + (ConnectedToSignalSender ? "Yes" : "No");
                return text;
            }
        }

        [JsonIgnore]
        protected string attatchmentString
        {
            get
            {
                string text = "";

                if (parent != null)
                {
                    text += "\n\nThe \"" + Name + "\" is attatched to the \"" + parent.Name + "\"";
                }

                var parts = GetCollection<BodyPart>().Objects;
                if (parts.Length > 0)
                {
                    if (parent == null) text += "\n";
                    text += "\n" + parts.Length + " parts are attatched to the \"" + Name + "\"";
                    foreach (var item in parts)
                    {
                        text += "\n - \"" + item.Name + "\"";
                    }
                }

                return text;
            }
        }

        [JsonIgnore]
        protected string vesselString
        {
            get
            {
                string text = "";

                var vessels = GetCollection<BloodVessel>().Objects;
                if (vessels.Length == 0)
                {
                    text += "\n\nThe " + Name + " has no blood vessels";
                }
                else
                {
                    text += "\n\nThe " + Name + " has these blood vessels";

                    List<List<BloodVessel>> groupedVessels = new List<List<BloodVessel>>();

                    foreach (var bodypart in Game.LocateObjectsWithType<BodyPart>(false))
                    {
                        List<BloodVessel> partVessels = new List<BloodVessel>();
                        foreach (var item in bodypart.GetCollection<BloodVessel>().Objects)
                        {
                            if (item.target == this) partVessels.Add(item);
                        }
                        foreach (var item in vessels)
                        {
                            if (item.target == bodypart)
                            {
                                partVessels.Add(item);
                            }
                        }
                        if (partVessels.Count > 0) groupedVessels.Add(partVessels);
                    }

                    int longestWord = 0;
                    foreach (var item in groupedVessels)
                    {
                        int found = item.Max((a) => {
                            if (a.target == this)
                            {
                                return a.source.Name.Length;
                            }
                            else
                            {
                                return a.target.Name.Length;
                            }
                        });
                        if (found > longestWord) longestWord = found;
                    }

                    foreach (var vesselList in groupedVessels)
                    {
                        if (groupedVessels.IndexOf(vesselList) != 0) text += "\n";
                        foreach (var vessel in vesselList)
                        {
                            if (vessel.target != this)
                                text += "\n" + (" - to " + vessel.target.Name).Pad(' ', longestWord + 9) + "(" + vessel.Name + ")";
                            else
                                text += "\n" + (" - from " + vessel.source.Name).Pad(' ', longestWord + 9) + "(" + vessel.Name + ")";
                        }
                    }

                    int hanging = 0;
                    foreach (var vessel in vessels) if (vessel.hanging) hanging++;
                    if (hanging > 0)
                    {
                        text += "\n";
                        foreach (var vessel in vessels)
                        {
                            if (vessel.hanging)
                            {
                                text += "\n" + " - hanging " + vessel.Name;
                            }
                        }
                    }
                }

                return text;
            }
        }

        [JsonIgnore]
        protected string bodypartGameobjectString
        {
            get
            {
                return Name + ((ShortDescription == "") ? "" : " - " + ShortDescription) + (Settings.DebugMode ? $" [<{Color.Orange.ToInteger()},sever {ID}>Sever Part@]" : "") + ((Description == "") ? "" : "\n" + Description)
                    + equipedDescription + holdingDescription + inventoryDescription + objectsDescription;
            }
        }

        [JsonIgnore]
        public override string Examine
        {
            get
            {
                string text = "";

                text += bodypartGameobjectString;

                text += specifitcationString;

                text += attatchmentString;

                text += vesselString;

                text += effector.Examine;

                text += interactionString;

                return text;
            }
        }

        public void SeverPart(GameObject collection)
        {
            if (parent != null) parent.GetCollection<BodyPart>().RemoveObject(this);
            parent = null;
            collection.GetCollection<Item>().AddObject(this);
            foreach (var item in GetVesselsAttatchedToMe())
            {
                if (!item.source.IsChildOf(this))
                {
                    item.hanging = true;
                    item.target = null;
                }
            }
            foreach (var item in GetNervesAttatchedToMe())
            {
                if (!item.source.IsChildOf(this))
                {
                    item.hanging = true;
                    item.target = null;
                }
            }
            foreach (var item in GetCollection<BloodVessel>().Objects)
            {
                if (!item.target.IsChildOf(this))
                {
                    item.hanging = true;
                    item.target = null;
                }
            }
            foreach (var item in GetCollection<Nerve>().Objects)
            {
                if (!item.target.IsChildOf(this))
                {
                    item.hanging = true;
                    item.target = null;
                }
            }
            grabable = true;
        }

        public bool IsChildOf(BodyPart part)
        {
            if (parent == part) return true;
            else if (parent == null) return false;
            else return parent.IsChildOf(part);
        }

        public bool IsOnBodySide(BodyPart part)
        {
            if (part.parent == this) return false;
            if (container.Owner is Entity) return true;
            if (parent != null)
            {
                return IsOnBodySide(parent);
            }
            return false;
        }

        public BloodVessel[] GetVesselsAttatchedToMe()
        {
            List<BloodVessel> vessels = new List<BloodVessel>();

            foreach (var vessel in Game.LocateObjectsWithType<BloodVessel>(false))
            {
                if (vessel.target == this) vessels.Add(vessel);
            }

            return vessels.ToArray();
        }

        public Nerve[] GetNervesAttatchedToMe()
        {
            List<Nerve> nerves = new List<Nerve>();

            foreach (var nerve in Game.LocateObjectsWithType<Nerve>(false))
            {
                if (nerve is CyberneticConnection)
                {
                    if (!(nerve as CyberneticConnection).transmitsNerveInformation) continue;
                }
                if (nerve.target == this) nerves.Add(nerve);
            }

            return nerves.ToArray();
        }

        public bool ConnectedToSignalSender
        {
            get
            {
                var signalsenders = new List<BodyPart>();
                signalsenders.AddRange(Game.LocateObjectsWithType<Brain>(false));
                if (this is CyberneticBodyPart) signalsenders.AddRange(Game.LocateObjectsWithType<CyberSignalSender>(false));
                foreach (var sender in signalsenders)
                {
                    if (this is CyberneticBodyPart)
                    {
                        if (sender.RecursivleyCheckForDataConnection(this, new List<BodyPart>())) return true;
                    }
                    else if (sender.RecursivleyCheckForNervousConnection(this, new List<BodyPart>())) return true;
                }
                return false;
            }
        }

        public bool RecursivleyCheckForNervousConnection(BodyPart part, List<BodyPart> checkparts)
        {
            if (checkparts.Contains(this)) return false;
            checkparts.Add(this);
            if (part is CyberSignalSender || part is Brain) return true;
            foreach (var nerve in GetCollection<Nerve>().Objects)
            {
                if (nerve.target == null && nerve.hanging) continue;
                if (nerve is CyberneticConnection)
                {
                    if (!(nerve as CyberneticConnection).transmitsNerveInformation) continue;
                }
                if (nerve.target == part) return true;
                else if (nerve.target.RecursivleyCheckForNervousConnection(part, checkparts)) return true;
            }
            return false;
        }

        public bool RecursivleyCheckForDataConnection(BodyPart part, List<BodyPart> checkparts)
        {
            if (checkparts.Contains(this)) return false;
            checkparts.Add(this);
            if (part is CyberSignalSender || part is Brain) return true;
            foreach (var nerve in GetCollection<Nerve>().Objects)
            {
                if (nerve.target == null && nerve.hanging) continue;
                if (nerve is CyberneticConnection)
                {
                    if (nerve.target == part) return true;
                    else if (nerve.target.RecursivleyCheckForDataConnection(part, checkparts)) return true;
                }
            }
            return false;
        }

        public string BloodPressureState(float bp)
        {
            bp *= 100;
            if (bp >= 90) return "Strong";
            if (bp < 90 && bp >= 70) return "Good";
            if (bp < 70 && bp > 50) return "Irregular";
            if (bp < 50) return "Terrible";
            return "None!";
        }

        public void DamagePart(float amount)
        {
            damage = Math.Max(damage + amount, 100);
        }

        [JsonIgnore]
        public BodyPart[] Children
        {
            get
            {
                return GetCollection<BodyPart>().GetAllObjects();
            }
        }

        public BodyPart AttatchBodyPart(BodyPart part)
        {
            GetCollection<BodyPart>().AddObject(part);
            part.parent = this;
            return part;
        }

        public BodyPart DetatchBodyPart(BodyPart part, ICollection container)
        {
            if (GetCollection<BodyPart>().HasObject(part))
            {
                GetCollection<BodyPart>().RemoveObject(part);
                container.GetTypedCollection().AddObject(part);
                part.parent = null;
            }
            return part;
        }

        public BodyPart DetatchBodyPart(BodyPart part)
        {
            if (GetCollection<BodyPart>().HasObject(part))
            {
                part.parent = null;
            }
            return part;
        }

        public override void Update()
        {
            base.Update();

            if (usesBlood) ConsumeOxygen();

            float pxAvaliable = Utilities.Clamp(bloodData.oxygenatedBlood - bloodData.hypoBloodLevel, 0, float.MaxValue);
            float deOxAvaliable = bloodData.deoxygenatedBlood;

            ProcessVessels(pxAvaliable, deOxAvaliable);

            foreach (var i in Children)
            {
                i.Update();
            }

            effector.UpdateEffects();

        }

        public float damageOnOxygenMissing = 5;
        private void ConsumeOxygen()
        {
            float toUse = bloodData.oxygenUse;
            if (bloodData.oxygenatedBlood < toUse) toUse = bloodData.oxygenatedBlood;
            if (damageOnOxygenMissing > 0 && bloodData.oxygenatedBlood - toUse < bloodData.hypoBloodLevel)
            {
                float newOxy = Math.Max(bloodData.oxygenatedBlood - toUse, 0);
                float damage = 0;
                if (newOxy == 0) damage = damageOnOxygenMissing;
                else damage = Utilities.GetBetweenValues(damageOnOxygenMissing, 0, bloodData.oxygenatedBlood / bloodData.hypoBloodLevel);
                OnDamage(new DamageInfo
                {
                    damage = damage,
                    damageType = DamageType.physical,
                    sender = this,
                    target = this
                });
            }
            bloodData.oxygenatedBlood = Math.Max(bloodData.oxygenatedBlood - toUse, 0);
            bloodData.deoxygenatedBlood += toUse;
        }

        [JsonIgnore]
        public float InversedNormalizedDamage
        {
            get
            {
                if (damage == 0 || damageOnOxygenMissing == 0) return 1;
                return 1 - damage / 100;
            }
        }

        [JsonIgnore]
        public float NormalizedDamage
        {
            get
            {
                if (damage == 0 || damageOnOxygenMissing == 0) return 0;
                return damage / 100;
            }
        }

        public virtual void SetBloodPressure(float amount)
        {
            bloodData.bloodPressure = Utilities.Clamp(amount, 0f, 1f);
        }

        public void AddVessel(BloodVessel vessel, BodyPart target)
        {
            vessel.source = this;
            vessel.target = target;
            GetCollection<BloodVessel>().AddObject(vessel);
        }

        public void AddNerve(Nerve nerve, BodyPart target)
        {
            nerve.source = this;
            nerve.target = target;
            GetCollection<Nerve>().AddObject(nerve);
        }

        public void PrefillWithBlood()
        {
            PrefillWithBlood(0.8f, 0.2f);
        }

        public void PrefillWithBlood(float OxPerc, float DeOxPerc)
        {
            bloodData.oxygenatedBlood = Utilities.GetBetweenValues(bloodData.hypoBloodLevel, bloodData.hyperBloodLevel, OxPerc);
            bloodData.deoxygenatedBlood = Utilities.GetBetweenValues(bloodData.hypoBloodLevel, bloodData.hyperBloodLevel, DeOxPerc);
        }

        private void ProcessVessels(float oxygenated, float deoxygenated, string debug = "")
        {
            var vessels = GetCollection<BloodVessel>();
            if (vessels.Objects.Length == 0) return;

            float arteryCapacity = vessels.ObjectList.FindAll((a) => a.movesOxygenated).Sum((a) => a.capacity);
            float veinCapacity = vessels.ObjectList.FindAll((a) => !a.movesOxygenated).Sum((a) => a.capacity);

            foreach (var vessel in vessels.Objects)
            {
                float bloodToMove = Utilities.Clamp((vessel.movesOxygenated ? oxygenated : deoxygenated) * (vessel.capacity / (vessel.movesOxygenated ? arteryCapacity : veinCapacity)) * bloodData.bloodPressure * InversedNormalizedDamage, 0, vessel.capacity);
                if (vessel.movesOxygenated)
                {
                    if (vessel.target.bloodData.oxygenatedBlood > vessel.target.bloodData.hyperBloodLevel) bloodToMove *= 0.25f;
                    bloodData.oxygenatedBlood -= bloodToMove;
                    if (vessel.target == null && vessel.hanging) continue;
                    vessel.target.bloodData.oxygenatedBlood += bloodToMove;
                }
                else
                {
                    bloodData.deoxygenatedBlood -= bloodToMove;
                    if (vessel.target == null && vessel.hanging) continue;
                    vessel.target.bloodData.deoxygenatedBlood += bloodToMove;
                }
                if (vessel.transfersBloodPressure && vessel.target != null && !vessel.hanging) vessel.target.SetBloodPressure(bloodData.bloodPressure - bloodData.bloodPressureFalloff);
            }
        }

        public void UpdateEffects()
        {
            effector.UpdateEffects();
        }

        public void OnDamage(DamageInfo damageInfo)
        {
            ((IDamagable)damager).OnDamage(damageInfo);
        }

        public void AddEffect(Effect effect)
        {
            ((IEffectable)effector).AddEffect(effect);
        }

        public bool HasEffect(Effect effect)
        {
            return ((IEffectable)effector).HasEffect(effect);
        }

        public bool EffectCanBeUsedOn(Effect effect)
        {
            return ((IEffectable)effector).EffectCanBeUsedOn(effect);
        }

        public bool CanEquipItem(Equipable equipable)
        {
            return ((IEquipable)equipmentManager).CanEquipItem(equipable);
        }

        public Item EquipItem(Equipable equipable)
        {
            return ((IEquipable)equipmentManager).EquipItem(equipable);
        }

        public DamageLevel DamageLevel => ((IDamagable)damager).DamageLevel;

        public float TotalBlood
        {
            get
            {
                return bloodData.oxygenatedBlood + bloodData.deoxygenatedBlood;
            }
        }

        [JsonIgnore]
        public bool HasItemEquiped => ((IEquipable)equipmentManager).HasItemEquiped;

        [JsonIgnore]
        public Item EquipedItem => ((IEquipable)equipmentManager).EquipedItem;
    }
}
