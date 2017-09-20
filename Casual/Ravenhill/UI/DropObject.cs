using Casual.Ravenhill.Data;
using Casual.UI;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Casual.Ravenhill.UI {
    public partial class DropObject : RavenhillGameBehaviour {

        private DropItem dropItem = null;
        private bool isCollected = false;

        public void Setup(DropItem dropItem, Transform sourceTransform = null) {
            this.dropItem = dropItem;

            icon.overrideSprite = resourceService.GetSprite(dropItem);
            icon.color = dropItem.color;

            trigger.SetEventTriggerClick(eventData => MoveToEndPosition(), engine.GetService<IAudioService>());

            Vector2 startPosition = sourceTransform ? canvasService.WorldToCanvasPoint(sourceTransform.position) : dropDefaultPosition;

            gameObject.GetOrAdd<Bezier2Movable>().Move(startPosition, 
                DropMidPosition(startPosition), 
                DropEndPosition(startPosition), 
                dropSpeed, MoveToEndPosition);

            gameObject.GetOrAdd<RectTransformAnimScale>().StartAnim(new MCFloatAnimData {
                duration = 0.8f,
                start = 1.0f,
                end = 1.5f,
                endAction = () => {
                    gameObject.GetOrAdd<RectTransformAnimScale>().StartAnim(new MCFloatAnimData {
                        duration = 2.0f,
                        start = GetComponent<RectTransform>().localScale.x,
                        end = 0.4f,
                        endAction = () => { },
                    });
                },
            });

            if(dropItem.isCreateScreenText) {
                CreateScreenText(dropItem, sourceTransform);
            }

            engine.GetService<IAudioService>().PlaySound(SoundType.drop_object, GetComponent<AudioSource>());
        }

        private readonly Dictionary<DropType, Color> dropTextColors = new Dictionary<DropType, Color> {
            [DropType.exp] = Color.yellow,
            [DropType.silver] = Color.blue,
            [DropType.health] = Color.magenta,
            [DropType.gold] = Utility.RGBA(193, 95, 31),
            [DropType.item] = Color.green,
            [DropType.max_health] = Utility.RGBA(102, 0, 51)
        };

        private void CreateScreenText(DropItem dropItem, Transform sourceTransform) {
            Vector2 screenPosition = (sourceTransform != null) ? 
                Camera.main.WorldToScreenPoint(sourceTransform.position) : 
                new Vector3(Screen.width / 2, Screen.height / 2);
            Color color = Color.white;
            if(dropTextColors.ContainsKey(dropItem.type) ) {
                color = dropTextColors[dropItem.type];
            }
            viewService.CreateScreenText("+" + dropItem.count.ToString(), screenPosition, color);
        }

        private Vector2 dropDefaultPosition {
            get {
                return Utility.Range(dropDefaultPositionMin, dropDefaultPositionMax);
            }
        }

        private Vector2 DropMidPosition(Vector2 startPosition) {
            return startPosition + Utility.Range(dropMidPositionMin, dropMidPositionMax);
        }

        private Vector2 DropEndPosition(Vector2 startPosition) {
            return startPosition + Utility.Range(dropEndPositionMin, dropEndPositionMax);
        }

        private Vector2 MoveMidPosition(Vector2 sourcePosition) {
            return sourcePosition + Utility.Range(moveMidPositionMin, moveMidPositionMax);
        }

        private Vector2 MoveEndPosition(Vector2 sourcePosition) {
            return sourcePosition + Utility.Range(moveEndPositionMin, moveEndPositionMax);
        }

        private void MoveToEndPosition() {
            if(!isCollected) {
                isCollected = true;
                Vector2 sourcePosirion = GetComponent<RectTransform>().anchoredPosition;
                gameObject.GetOrAdd<Bezier2Movable>().Move(sourcePosirion,
                    MoveMidPosition(sourcePosirion),
                    MoveEndPosition(sourcePosirion),
                    moveSpeed, () => {
                        Destroy(gameObject);
                    });
                PlayTakeSound();
            }
        }

        private void PlaySound(SoundType soundType) {
            engine.GetService<IAudioService>().PlaySound(soundType, GetComponent<AudioSource>());
        }

        private void PlayTakeSound() {
            switch(dropItem.type) {
                case DropType.exp: {
                        PlaySound(SoundType.take_exp);
                    }
                    break;
                case DropType.health:
                case DropType.max_health: {
                        PlaySound(SoundType.take_health);
                    }
                    break;
                case DropType.silver:
                case DropType.gold: {
                        PlaySound(SoundType.take_money);
                    }
                    break;
                default: {
                        PlaySound(SoundType.take_exp);
                    }
                    break;
            }
        }

        public override void OnDisable() {
            base.OnDisable();
            GiveItem();
        }

        protected virtual void GiveItem() {
            switch(dropItem.type) {
                case DropType.exp: {
                        playerService.AddExp(dropItem.count);
                    }
                    break;
                case DropType.gold: {
                        playerService.AddGold(dropItem.count);
                    }
                    break;
                case DropType.health: {
                        playerService.AddHealth(dropItem.count);
                    }
                    break;
                case DropType.item: {
                        playerService.AddToInventory(new InventoryItem(dropItem.itemData, dropItem.count));
                    }
                    break;
                case DropType.max_health: {
                        playerService.AddMaxHealth(dropItem.count);
                    }
                    break;
                case DropType.silver: {
                        playerService.AddSilver(dropItem.count);
                    }
                    break;
            }
        }

        public static DropObject Create(DropItem dropItem, Transform sourceTransform = null ) {
            GameObject prefab = CasualEngine.Get<RavenhillEngine>()
                .GetService<IResourceService>()
                .Cast<RavenhillResourceService>()
                .GetCachedPrefab("drop_object");
            GameObject instance = GameObject.Instantiate<GameObject>(prefab);
            CanvasService canvasService = CasualEngine.Get<RavenhillEngine>()
                .GetService<ICanvasSerive>()
                .Cast<CanvasService>();
            canvasService.AddToFirstGroup(instance.transform);
            DropObject dropObject = instance.GetComponent<DropObject>();
            dropObject.Setup(dropItem, sourceTransform);
            return dropObject;
        }
    }

    public partial class DropObject : RavenhillGameBehaviour {
#pragma warning disable 0649
        [SerializeField]
        private Image icon;

        [SerializeField]
        private EventTrigger trigger;

        [SerializeField]
        private Vector2 dropDefaultPositionMin;

        [SerializeField]
        private Vector2 dropDefaultPositionMax;

        [SerializeField]
        private Vector2 dropMidPositionMin;

        [SerializeField]
        private Vector2 dropMidPositionMax;

        [SerializeField]
        private Vector2 dropEndPositionMin;

        [SerializeField]
        private Vector2 dropEndPositionMax;

        [SerializeField]
        private float dropSpeed;

        [SerializeField]
        private Vector2 moveMidPositionMin;

        [SerializeField]
        private Vector2 moveMidPositionMax;


        [SerializeField]
        private Vector2 moveEndPositionMin;

        [SerializeField]
        private Vector2 moveEndPositionMax;

        [SerializeField]
        private float moveSpeed;

#pragma warning restore 0649
    }
}
