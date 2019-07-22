using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZorkLike.Organs;
using ZorkLike.Effects;

namespace ZorkLike.Entities
{
    class Humanoid : Entity
    {

        public Humanoid(string name, string[] ids) : base(name, ids)
        {
            AddCollection<BodyPart>();
            var parts = GetCollection<BodyPart>();

            var head = (Head)parts.AddObject(new Head());
            var chest = (Extremity)head.AttatchBodyPart(new Extremity("Chest", "The cavity for which allot of your critical organs reside.", new string[] { "chest", "body" }));
            var left_lung = (Lung)chest.AttatchBodyPart(new Lung("Left Lung", 0.8f, 10f, new string[] { "left lung", "lung" })); // Left lung is slightly smaller.
            var right_lung = (Lung)chest.AttatchBodyPart(new Lung("Right Lung", 0.9f, 12f, new string[] { "right lung", "lung" }));
            var heart = (Heart)chest.AttatchBodyPart(new Heart());
            var stomach = (Stomach)chest.AttatchBodyPart(new Stomach());

            //left_lung.AddEffect(new Combat.Reverse());

            chest.AddEffect(new Burn());

            chest.PrefillWithBlood();
            chest.bloodData.hyperBloodLevel = 1000f;
            heart.lowerHeartPressure = 0.9f;
            heart.maximumHeartPressure = 1f;

            head.PrefillWithBlood();
            heart.PrefillWithBlood();
            left_lung.PrefillWithBlood();
            right_lung.PrefillWithBlood();
            stomach.PrefillWithBlood();

            heart.AddVessel(new LargePulmonaryArtery() { capacity = 16 }, left_lung);
            heart.AddVessel(new LargePulmonaryArtery() { capacity = 16 }, right_lung);
            heart.AddVessel(new LargeArtery() { capacity = 32 }, chest);

            left_lung.AddVessel(new LargeArtery(), heart);
            right_lung.AddVessel(new LargeArtery(), heart);

            chest.AddVessel(new Artery(), head);
            chest.AddVessel(new Capillary(), heart);
            chest.AddVessel(new LargeVein() { capacity = 32 }, heart);
            chest.AddVessel(new Artery(), stomach);

            head.AddVessel(new Vein(), chest);
            head.AddVessel(new Capillary(), chest);

            stomach.AddVessel(new Vein(), chest);
            stomach.AddVessel(new Capillary(), chest);

            var left_upper_arm = chest.AttatchBodyPart(new Upper_Arm("Left Upper Arm", new string[] { "upper arm", "left upper arm", "arm" }));
            chest.AddVessel(new LargeArtery(), left_upper_arm);
            left_upper_arm.AddVessel(new LargeVein(), chest);
            left_upper_arm.AddVessel(new LargeCapillary(), chest);
            left_upper_arm.PrefillWithBlood();

            var left_lower_arm = left_upper_arm.AttatchBodyPart(new Lower_Arm("Left Lower Arm", new string[] { "lower arm", "left lower arm", "arm" }));
            left_upper_arm.AddVessel(new LargeArtery(), left_lower_arm);
            left_lower_arm.AddVessel(new LargeVein(), left_upper_arm);
            left_lower_arm.AddVessel(new LargeCapillary(), left_upper_arm);
            left_lower_arm.PrefillWithBlood();

            var left_hand = left_lower_arm.AttatchBodyPart(new Hand("Left Hand", new string[] { "hand", "left hand" }));
            left_lower_arm.AddVessel(new LargeArtery(), left_hand);
            left_hand.AddVessel(new LargeVein(), left_lower_arm);
            left_hand.AddVessel(new LargeCapillary(), left_lower_arm);
            left_hand.PrefillWithBlood();

            var right_upper_arm = chest.AttatchBodyPart(new Upper_Arm("Right Upper Arm", new string[] { "upper arm", "right upper arm", "arm" }));
            chest.AddVessel(new LargeArtery(), right_upper_arm);
            right_upper_arm.AddVessel(new LargeVein(), chest);
            right_upper_arm.AddVessel(new LargeCapillary(), chest);
            right_upper_arm.PrefillWithBlood();

            var right_lower_arm = right_upper_arm.AttatchBodyPart(new Lower_Arm("Right Lower Arm", new string[] { "lower arm", "right lower arm", "arm" }));
            right_upper_arm.AddVessel(new LargeArtery(), right_lower_arm);
            right_lower_arm.AddVessel(new LargeVein(), right_upper_arm);
            right_lower_arm.AddVessel(new LargeCapillary(), right_upper_arm);
            right_lower_arm.PrefillWithBlood();

            //var right_hand = right_lower_arm.AttatchBodyPart(new Hand("Right Hand", new string[] { "hand", "right hand" }));
            //right_lower_arm.AddVessel(new LargeArtery(), right_hand);
            //right_hand.AddVessel(new LargeVein(), right_lower_arm);
            //right_hand.AddVessel(new LargeCapillary(), right_lower_arm);
            //right_hand.PrefillWithBlood();

            var right_hand = right_lower_arm.AttatchBodyPart(new CyberHand("Right Cyber Hand", new string[] { "hand", "right hand", "cyberware", "cybernetics", "right cyber hand", "cyber hand" }));

        }
    }
}
