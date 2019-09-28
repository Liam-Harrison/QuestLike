using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QuestLike.Organs;
using QuestLike.Effects;
using Newtonsoft.Json;

namespace QuestLike.Entities
{
    class PartialCyberHumanoid : Entity
    {

        public PartialCyberHumanoid(string name, string[] ids) : base(name, ids)
        {
            AddCollection<BodyPart>();
            var parts = GetCollection<BodyPart>();

            var brain = (Brain)parts.AddObject(new Brain());
            brain.PrefillWithBlood();

            var head = (Head)brain.AttatchBodyPart(new Head());
            brain.AddVessel(new LargeVein(), head);
            brain.AddVessel(new LargeCapillary(), head);
            brain.AddNerve(new CyberneticConnection(), head);
            head.AddVessel(new LargeArtery() { capacity = 16 }, brain);
            head.PrefillWithBlood();

            var chest = (Chest)head.AttatchBodyPart(new Chest("Chest", "The cavity for which allot of your critical organs reside.", 
                new string[] { "chest", "body" }));
            chest.bloodData.hyperBloodLevel = 1000f;
            chest.PrefillWithBlood();
            chest.bloodData.hyperBloodLevel = 2500f;
            chest.AddVessel(new LargeArtery(), head);
            head.AddNerve(new CyberneticConnection(), chest);
            head.AddVessel(new LargeVein(), chest);
            head.AddVessel(new LargeCapillary(), chest);

            var heart = (Heart)chest.AttatchBodyPart(new Heart());
            chest.AddVessel(new Capillary(), heart);
            chest.AddVessel(new LargeVein() { capacity = 32 }, heart);
            chest.AddNerve(new Nerve(), heart);
            heart.AddVessel(new LargeArtery() { capacity = 48 }, chest);
            heart.lowerHeartPressure = 0.9f;
            heart.maximumHeartPressure = 1f;
            heart.PrefillWithBlood();

            var abdomen = (Extremity)chest.AttatchBodyPart(new Extremity("Lower Abdomen", new string[] { "lower chest", "abdomen",
                "lower abdomen", "body" }));
            abdomen.bloodData.hyperBloodLevel = 750f;
            abdomen.PrefillWithBlood();
            abdomen.bloodData.hyperBloodLevel = 1500f;
            chest.AddNerve(new Nerve(), abdomen);
            chest.AddVessel(new LargeArtery() { capacity = 32 }, abdomen);
            abdomen.AddVessel(new LargeVein() { capacity = 32 }, chest);
            abdomen.AddVessel(new LargeCapillary() { capacity = 32 }, chest);

            var left_lung = (Lung)chest.AttatchBodyPart(new Lung("Left Lung", 0.85f, 14f, new string[] { "left lung", "lung" }));
            // Left lung is slightly smaller than the right.
            heart.AddVessel(new LargePulmonaryArtery() { capacity = 16 }, left_lung);
            chest.AddNerve(new Nerve(), left_lung);
            left_lung.AddVessel(new LargeArtery(), heart);
            left_lung.PrefillWithBlood();

            var right_lung = (Lung)chest.AttatchBodyPart(new Lung("Right Lung", 0.9f, 15f, new string[] { "right lung", "lung" }));
            heart.AddVessel(new LargePulmonaryArtery() { capacity = 16 }, right_lung);
            chest.AddNerve(new Nerve(), right_lung);
            right_lung.AddVessel(new LargeArtery(), heart);
            right_lung.PrefillWithBlood();

            var stomach = (Stomach)chest.AttatchBodyPart(new Stomach());
            chest.AddVessel(new Artery(), stomach);
            stomach.AddVessel(new Vein(), chest);
            stomach.AddVessel(new Capillary(), chest);
            stomach.PrefillWithBlood();

            var left_upper_arm = chest.AttatchBodyPart(new Upper_Arm("Left Upper Arm", new string[] { "upper arm", "left upper arm", "arm" }));
            chest.AddVessel(new LargeArtery(), left_upper_arm);
            left_upper_arm.AddVessel(new LargeVein(), chest);
            left_upper_arm.AddVessel(new LargeCapillary(), chest);
            chest.AddNerve(new Nerve(), left_upper_arm);
            left_upper_arm.PrefillWithBlood();

            var left_lower_arm = left_upper_arm.AttatchBodyPart(new Lower_Arm("Left Lower Arm", new string[] { "lower arm", "left lower arm", "arm" }));
            left_upper_arm.AddVessel(new LargeArtery(), left_lower_arm);
            left_lower_arm.AddVessel(new LargeVein(), left_upper_arm);
            left_lower_arm.AddVessel(new LargeCapillary(), left_upper_arm);
            left_upper_arm.AddNerve(new Nerve(), left_lower_arm);
            left_lower_arm.PrefillWithBlood();

            var left_hand = left_lower_arm.AttatchBodyPart(new Hand("Left Hand", "A meat hand", "", new string[] { "hand", "left hand" }));
            left_lower_arm.AddVessel(new LargeArtery(), left_hand);
            left_hand.AddVessel(new LargeVein(), left_lower_arm);
            left_hand.AddVessel(new LargeCapillary(), left_lower_arm);
            left_lower_arm.AddNerve(new Nerve(), left_hand);
            left_hand.PrefillWithBlood();

            var right_upper_arm = chest.AttatchBodyPart(new Upper_Arm("Right Upper Arm", new string[] { "upper arm", "right upper arm", "arm" }));
            chest.AddVessel(new LargeArtery(), right_upper_arm);
            right_upper_arm.AddVessel(new LargeVein(), chest);
            right_upper_arm.AddVessel(new LargeCapillary(), chest);
            chest.AddNerve(new CyberneticConnection(), right_upper_arm);
            right_upper_arm.PrefillWithBlood();

            var right_lower_arm = right_upper_arm.AttatchBodyPart(new Lower_Arm("Right Lower Arm", new string[] { "lower arm", "right lower arm",
                "arm" }));
            right_upper_arm.AddVessel(new LargeArtery(), right_lower_arm);
            right_lower_arm.AddVessel(new LargeVein(), right_upper_arm);
            right_lower_arm.AddVessel(new LargeCapillary(), right_upper_arm);
            right_upper_arm.AddNerve(new CyberneticConnection(), right_lower_arm);
            right_lower_arm.PrefillWithBlood();

            var right_hand = right_lower_arm.AttatchBodyPart(new CyberHand("Right Cyber Hand", "A cybernetic hand", "", new string[] { "hand", "right hand", "cyberware",
                "cybernetics", "right cyber hand", "cyber hand" }));
            right_lower_arm.AddNerve(new CyberneticConnection(), right_hand);

            var left_upper_leg = abdomen.AttatchBodyPart(new Upper_Leg("Left Upper Leg", new string[] { "leg", "upper leg", "left upper leg",
                "left leg" }));
            abdomen.AddVessel(new LargeArtery(), left_upper_leg);
            abdomen.AddNerve(new Nerve(), left_upper_leg);
            left_upper_leg.AddVessel(new LargeVein(), abdomen);
            left_upper_leg.AddVessel(new LargeCapillary(), abdomen);
            left_upper_leg.PrefillWithBlood();

            var left_lower_leg = left_upper_leg.AttatchBodyPart(new Lower_Leg("Left Lower Leg", new string[] { "leg", "lower leg",
                "left lower leg", "left lower legs", "legs" }));
            left_upper_leg.AddVessel(new LargeArtery(), left_lower_leg);
            left_upper_leg.AddNerve(new Nerve(), left_lower_leg);
            left_lower_leg.AddVessel(new LargeVein(), left_upper_leg);
            left_lower_leg.AddVessel(new LargeCapillary(), left_upper_leg);
            left_upper_leg.PrefillWithBlood();

            var left_foot = left_lower_leg.AttatchBodyPart(new Foot("Left Foot", new string[] { "foot", "left foot", "feet" }));
            left_lower_leg.AddVessel(new LargeArtery(), left_foot);
            left_lower_leg.AddNerve(new Nerve(), left_foot);
            left_foot.AddVessel(new LargeVein(), left_lower_leg);
            left_foot.AddVessel(new LargeCapillary(), left_lower_leg);
            left_foot.PrefillWithBlood();

            var right_upper_leg = abdomen.AttatchBodyPart(new Upper_Leg("Right Upper Leg", new string[] { "leg", "upper leg", "right upper leg",
                "right leg" }));
            abdomen.AddVessel(new LargeArtery(), right_upper_leg);
            abdomen.AddNerve(new Nerve(), right_upper_leg);
            right_upper_leg.AddVessel(new LargeVein(), abdomen);
            right_upper_leg.AddVessel(new LargeCapillary(), abdomen);
            right_upper_leg.PrefillWithBlood();

            var right_lower_leg = right_upper_leg.AttatchBodyPart(new Lower_Leg("Right Lower Leg", new string[] { "leg", "lower leg",
                "right lower leg", "right lower legs", "legs" }));
            right_upper_leg.AddVessel(new LargeArtery(), right_lower_leg);
            right_upper_leg.AddNerve(new Nerve(), right_lower_leg);
            right_lower_leg.AddVessel(new LargeVein(), right_upper_leg);
            right_lower_leg.AddVessel(new LargeCapillary(), right_upper_leg);
            right_upper_leg.PrefillWithBlood();

            var right_foot = right_lower_leg.AttatchBodyPart(new Foot("Right Foot", new string[] { "foot", "right foot", "feet" }));
            right_lower_leg.AddVessel(new LargeArtery(), right_foot);
            right_lower_leg.AddNerve(new Nerve(), right_foot);
            right_foot.AddVessel(new LargeVein(), right_lower_leg);
            right_foot.AddVessel(new LargeCapillary(), right_lower_leg);
            right_foot.PrefillWithBlood();
        }
    }
}
