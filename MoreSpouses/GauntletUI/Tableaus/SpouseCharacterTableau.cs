using System;
using TaleWorlds.Core;
using TaleWorlds.Core.ViewModelCollection;
using TaleWorlds.Engine;
using TaleWorlds.InputSystem;
using TaleWorlds.Library;
using TaleWorlds.MountAndBlade;
using TaleWorlds.MountAndBlade.View;

namespace SueMoreSpouses.GauntletUI.Tableaus
{
    internal class SpouseCharacterTableau
    {
        private MatrixFrame _mountSpawnPoint;

        private float _animationFrequencyThreshold = 2.5f;

        private MatrixFrame _frame;

        private MatrixFrame _initialSpawnFrame;

        private AgentVisuals _agentVisuals;

        private GameEntity _mountEntity;

        private Scene _tableauScene;

        private MBAgentRendererSceneController _agentRendererSceneController;

        private Camera _continuousRenderCamera;

        private float _cameraRatio;

        private MatrixFrame _camPos;

        private bool _initialized;

        private int _tableauSizeX;

        private int _tableauSizeY;

        private uint _clothColor1 = new Color(1f, 1f, 1f, 1f).ToUnsignedInteger();

        private uint _clothColor2 = new Color(1f, 1f, 1f, 1f).ToUnsignedInteger();

        private bool _isRotatingCharacter;

        private string _mountCreationKey = "";

        private string _equipmentCode = "";

        private bool _isEquipmentAnimActive;

        private float _animationGap;

        private bool _isEnabled;

        private const float RenderScale = 1f;

        private string _bodyPropertiesCode;

        private BodyProperties _bodyProperties = BodyProperties.Default;

        private bool _isFemale;

        private CharacterViewModel.StanceTypes _stanceIndex;

        private Equipment _equipment;

        private Banner _banner;

        private static readonly ActionIndexCache act_character_developer_idle = ActionIndexCache.Create("act_character_developer_idle");

        private static readonly ActionIndexCache act_cheer_1 = ActionIndexCache.Create("act_cheer_1");

        private static readonly ActionIndexCache act_inventory_idle_start = ActionIndexCache.Create("act_inventory_idle_start");

        private static readonly ActionIndexCache act_inventory_glove_equip = ActionIndexCache.Create("act_inventory_glove_equip");

        private static readonly ActionIndexCache act_inventory_cloth_equip = ActionIndexCache.Create("act_inventory_cloth_equip");

        private static readonly ActionIndexCache act_horse_stand = ActionIndexCache.Create("act_inventory_idle_start");

        private static readonly ActionIndexCache act_camel_stand = ActionIndexCache.Create("act_inventory_idle_start");

        private bool _RightMousePressed;

        public Texture Texture
        {
            get;
            private set;
        }

        private TableauView View
        {
            get
            {
                bool flag = Texture != null;
                TableauView result;
                if (flag)
                {
                    result = Texture.TableauView;
                }
                else
                {
                    result = null;
                }
                return result;
            }
        }

        public SpouseCharacterTableau()
        {
            _equipment = new Equipment();
            SetEnabled(true);
        }

        private void SetEnabled(bool enabled)
        {
            _isEnabled = enabled;
            TableauView view = View;
            bool flag = view != null;
            if (flag)
            {
                view.SetEnable(_isEnabled);
            }
        }

        public void SetTargetSize(int width, int height)
        {
            _isRotatingCharacter = false;
            bool flag = width <= 0 || height <= 0;
            if (flag)
            {
                _tableauSizeX = 10;
                _tableauSizeY = 10;
            }
            else
            {
                _tableauSizeX = (int)(width * 1f);
                _tableauSizeY = (int)(height * 1f);
            }
            _cameraRatio = _tableauSizeX / (float)_tableauSizeY;
            TableauView view = View;
            bool flag2 = view != null;
            if (flag2)
            {
                view.SetEnable(false);
            }
            //this.Texture = TableauView.AddTableau(new RenderTargetComponent.TextureUpdateEventHandler(this.CharacterTableauContinuousRenderFunction), this._tableauScene, this._tableauSizeX, this._tableauSizeY);
            Texture = TableauView.AddTableau("CharacterTableau", new RenderTargetComponent.TextureUpdateEventHandler(CharacterTableauContinuousRenderFunction), _tableauScene, _tableauSizeX, _tableauSizeY);
        }


        public void OnFinalize()
        {
            bool flag = _continuousRenderCamera != null;
            if (flag)
            {
                _continuousRenderCamera.ReleaseCameraEntity();
                _continuousRenderCamera = null;
            }
            AgentVisuals agentVisuals = _agentVisuals;
            bool flag2 = agentVisuals != null;
            if (flag2)
            {
                agentVisuals.Reset();
            }
            _agentVisuals = null;
            TableauView view = View;
            bool flag3 = view != null;
            if (flag3)
            {
                view.AddClearTask(false);
            }
            Texture = null;
            bool flag4 = _tableauScene != null;
            if (flag4)
            {
                MBAgentRendererSceneController.DestructAgentRendererSceneController(_tableauScene, _agentRendererSceneController);
                _agentRendererSceneController = null;
                _tableauScene = null;
            }
        }

        public void SetBodyProperties(string bodyPropertiesCode)
        {
            bool flag = _bodyPropertiesCode != bodyPropertiesCode;
            if (flag)
            {
                _bodyPropertiesCode = bodyPropertiesCode;
                BodyProperties bodyProperties = default;
                bool flag2 = !string.IsNullOrEmpty(bodyPropertiesCode) && BodyProperties.FromString(bodyPropertiesCode, out bodyProperties);
                if (flag2)
                {
                    _bodyProperties = bodyProperties;
                }
                else
                {
                    _bodyProperties = BodyProperties.Default;
                }
                _initialized = false;
                RefreshCharacterTableau(null);
                ForceRefresh();
            }
        }

        public void SetStanceIndex(int index)
        {
            bool flag = _initialized && _stanceIndex != (CharacterViewModel.StanceTypes)index;
            if (flag)
            {
                switch (index)
                {
                    case 0:
                        _agentVisuals.SetAction(act_inventory_idle_start, 0f);
                        break;
                    case 1:
                        _camPos = _tableauScene.ReadAndCalculateInitialCamera();
                        _camPos.Elevate(-2f);
                        _camPos.Advance(0.5f);
                        break;
                    case 2:
                    case 4:
                        {
                            bool flag2 = _agentVisuals != null;
                            if (flag2)
                            {
                                _camPos = _tableauScene.ReadAndCalculateInitialCamera();
                                bool flag3 = _equipment[10].Item != null;
                                if (flag3)
                                {
                                    _camPos.Advance(0.5f);
                                    AddMount(true);
                                    _agentVisuals.SetAction(_mountEntity.Skeleton.GetActionAtChannel(0), _mountEntity.Skeleton.GetAnimationParameterAtChannel(0));
                                    for (int i = 0; i < 4; i++)
                                    {
                                        _mountEntity.Skeleton.TickAnimationsAndForceUpdate(0.1f, _frame, true);
                                        _agentVisuals.GetVisuals().GetSkeleton().TickAnimationsAndForceUpdate(0.1f, _frame, true);
                                    }
                                }
                                else
                                {
                                    _camPos.Elevate(-2f);
                                    _camPos.Advance(0.5f);
                                    _agentVisuals.SetAction(act_inventory_idle_start, 0f);
                                }
                            }
                            break;
                        }
                    case 3:
                        {
                            AgentVisuals agentVisuals = _agentVisuals;
                            bool flag4 = agentVisuals != null;
                            if (flag4)
                            {
                                agentVisuals.SetAction(act_cheer_1, 0f);
                            }
                            break;
                        }
                }
            }
            _stanceIndex = (CharacterViewModel.StanceTypes)index;
        }

        private void ForceRefresh()
        {
            bool initialized = _initialized;
            if (initialized)
            {
                int stanceIndex = (int)_stanceIndex;
                _stanceIndex = CharacterViewModel.StanceTypes.None;
                SetStanceIndex(stanceIndex);
            }
        }

        public void SetIsFemale(bool isFemale)
        {
            _isFemale = isFemale;
        }

        public void SetEquipmentCode(string equipmentCode)
        {
            bool flag = _equipmentCode != equipmentCode && !string.IsNullOrEmpty(equipmentCode);
            if (flag)
            {
                Equipment oldEquipment = Equipment.CreateFromEquipmentCode(_equipmentCode);
                _equipmentCode = equipmentCode;
                _equipment = Equipment.CreateFromEquipmentCode(equipmentCode);
                RefreshCharacterTableau(oldEquipment);
            }
            else
            {
                bool flag2 = equipmentCode == null;
                if (flag2)
                {
                }
            }
        }

        public void SetIsEquipmentAnimActive(bool value)
        {
            _isEquipmentAnimActive = value;
        }

        public void SetMountCreationKey(string value)
        {
            bool flag = _mountCreationKey != value;
            if (flag)
            {
                _mountCreationKey = value;
                RefreshCharacterTableau(null);
            }
        }

        public void SetBannerCode(string value)
        {
            bool flag = string.IsNullOrEmpty(value);
            if (flag)
            {
                _banner = null;
            }
            else
            {
                _banner = BannerCode.CreateFrom(value).CalculateBanner();
            }
            RefreshCharacterTableau(null);
        }

        public void SetArmorColor1(uint clothColor1)
        {
            bool flag = _clothColor1 != clothColor1;
            if (flag)
            {
                _clothColor1 = clothColor1;
                RefreshCharacterTableau(null);
            }
        }

        public void SetArmorColor2(uint clothColor2)
        {
            bool flag = _clothColor2 != clothColor2;
            if (flag)
            {
                _clothColor2 = clothColor2;
                RefreshCharacterTableau(null);
            }
        }

        private void RefreshCharacterTableau(Equipment oldEquipment = null)
        {
            bool flag = !_initialized;
            if (flag)
            {
                FirstTimeInit();
            }
            bool initialized = _initialized;
            if (initialized)
            {
                bool flag2 = _agentVisuals != null;
                if (flag2)
                {
                    AgentVisualsData copyAgentVisualsData = _agentVisuals.GetCopyAgentVisualsData();
                    copyAgentVisualsData.BodyProperties(_bodyProperties).SkeletonType(_isFemale ? SkeletonType.Female : SkeletonType.Male).Frame(_initialSpawnFrame).ActionSet(MBGlobals.HumanWarriorActionSet).Equipment(_equipment).Banner(_banner).UseMorphAnims(true).ClothColor1(_clothColor1).ClothColor2(_clothColor2);
                    _agentVisuals.Refresh(false, copyAgentVisualsData);
                    bool flag3 = oldEquipment != null && _animationFrequencyThreshold <= _animationGap && _isEquipmentAnimActive;
                    if (flag3)
                    {
                        bool flag4 = _equipment[EquipmentIndex.Gloves].Item != null && oldEquipment[EquipmentIndex.Gloves].Item != _equipment[EquipmentIndex.Gloves].Item;
                        if (flag4)
                        {
                            _agentVisuals.GetVisuals().GetSkeleton().SetAgentActionChannel(0, act_inventory_glove_equip, 0f, -0.2f);
                            _animationGap = 0f;
                        }
                        else
                        {
                            bool flag5 = _equipment[EquipmentIndex.Body].Item != null && oldEquipment[EquipmentIndex.Body].Item != _equipment[EquipmentIndex.Body].Item;
                            if (flag5)
                            {
                                _agentVisuals.GetVisuals().GetSkeleton().SetAgentActionChannel(0, act_inventory_cloth_equip, 0f, -0.2f);
                                _animationGap = 0f;
                            }
                        }
                    }
                }
                bool flag6 = _equipment[10].Item != null;
                if (flag6)
                {
                    AddMount(_stanceIndex == CharacterViewModel.StanceTypes.OnMount);
                }
                else
                {
                    RemoveMount();
                }
            }
        }

        public void RotateCharacter(bool value)
        {
            _isRotatingCharacter = value;
        }

        public void OnTick(float dt)
        {
            TickUserInputs(dt);
            bool flag = _isEnabled && _isRotatingCharacter;
            if (flag)
            {
                UpdateCharacterRotation((int)Input.MouseMoveX);
            }
            bool flag2 = _animationFrequencyThreshold > _animationGap;
            if (flag2)
            {
                _animationGap += dt;
            }
            bool flag3 = _isEnabled && _agentVisuals != null;
            if (flag3)
            {
                _agentVisuals.TickVisuals();
            }
            TableauView view = View;
            bool flag4 = view != null;
            if (flag4)
            {
                bool flag5 = _continuousRenderCamera == null;
                if (flag5)
                {
                    _continuousRenderCamera = Camera.CreateCamera();
                }
                view.SetDoNotRenderThisFrame(false);
            }
        }

        private void TickUserInputs(float dt)
        {
            bool flag = Input.DeltaMouseScroll != 0f;
            if (flag)
            {
                float a = Input.DeltaMouseScroll / 720f;
                _camPos.Elevate(a);
            }
            bool flag2 = InputKey.RightMouseButton.IsPressed();
            if (flag2)
            {
                _RightMousePressed = true;
            }
            bool flag3 = InputKey.RightMouseButton.IsReleased();
            if (flag3)
            {
                _RightMousePressed = false;
            }
            bool rightMousePressed = _RightMousePressed;
            if (rightMousePressed)
            {
                GameEntity entity = _agentVisuals.GetEntity();
                _frame = entity.GetFrame();
                _frame.rotation.RotateAboutSide(-Input.MouseMoveY * 3.14159274f / 720f);
                entity.SetFrame(ref _frame);
            }
        }

        public void OnCharacterTableauMouseMove(int mouseMoveX)
        {
            UpdateCharacterRotation(mouseMoveX);
        }

        private void UpdateCharacterRotation(int mouseMoveX)
        {
            bool flag = _initialized && _agentVisuals != null;
            if (flag)
            {
                GameEntity entity = _agentVisuals.GetEntity();
                _frame = entity.GetFrame();
                _frame.rotation.RotateAboutUp(mouseMoveX * 3.14159274f / 720f);
                entity.SetFrame(ref _frame);
            }
        }

        private void FirstTimeInit()
        {
            bool flag = _equipment != null;
            if (flag)
            {
                bool flag2 = _tableauScene == null;
                if (flag2)
                {
                    _tableauScene = Scene.CreateNewScene(true);
                    _tableauScene.SetName("CharacterTableau");
                    _tableauScene.DisableStaticShadows(true);
                    _agentRendererSceneController = MBAgentRendererSceneController.CreateNewAgentRendererSceneController(_tableauScene, 32);
                    SceneInitializationData initData = new SceneInitializationData(true);
                    initData.InitPhysicsWorld = false;
                    _tableauScene.Read("inventory_character_scene", initData, "");
                    _tableauScene.SetShadow(true);
                    _camPos = _tableauScene.ReadAndCalculateInitialCamera();
                    _mountSpawnPoint = _tableauScene.FindEntityWithTag("horse_inv").GetGlobalFrame();
                    MatrixFrame globalFrame = _tableauScene.FindEntityWithTag("agent_inv").GetGlobalFrame();
                    _frame = globalFrame;
                    _initialSpawnFrame = globalFrame;
                    _tableauScene.RemoveEntity(_tableauScene.FindEntityWithTag("agent_inv"), 99);
                    _tableauScene.RemoveEntity(_tableauScene.FindEntityWithTag("horse_inv"), 100);
                }
                bool flag3 = _agentVisuals != null;
                if (flag3)
                {
                    _agentVisuals.Reset();
                    _agentVisuals = null;
                }
                bool flag4 = _bodyProperties != BodyProperties.Default;
                if (flag4)
                {
                    _agentVisuals = AgentVisuals.Create(new AgentVisualsData().Banner(_banner).Equipment(_equipment).BodyProperties(_bodyProperties).Frame(_frame).UseMorphAnims(true).ActionSet(MBGlobals.HumanWarriorActionSet).ActionCode(act_inventory_idle_start).Scene(_tableauScene).Monster(Game.Current.HumanMonster).PrepareImmediately(true).SkeletonType(_isFemale ? SkeletonType.Female : SkeletonType.Male).ClothColor1(_clothColor1).ClothColor2(_clothColor2), "CharacterTableaue", true, false);
                    //_agentVisuals.SetAgentLodLevelExternal(0f);
                }
                _initialized = true;
            }
        }

        private void AddMount(bool isRiderAgentMounted = false)
        {
            RemoveMount();
            HorseComponent horseComponent = _equipment[10].Item.HorseComponent;
            Monster monster = horseComponent.Monster;
            _mountEntity = GameEntity.CreateEmpty(_tableauScene, true);
            AnimationSystemData animationSystemData = monster.FillAnimationSystemData(MBGlobals.GetActionSet(horseComponent.Monster.ActionSetCode), 1f, false);
            AgentVisualsNativeData agentVisualsNativeData = monster.FillAgentVisualsNativeData();
            _mountEntity.CreateSkeletonWithActionSet(ref agentVisualsNativeData, ref animationSystemData);
            if (isRiderAgentMounted)
            {
                _mountEntity.Skeleton.SetAgentActionChannel(0, horseComponent.Monster.MonsterUsage == "camel" ? act_camel_stand : act_horse_stand, MBRandom.RandomFloat, -0.2f);
            }
            else
            {
                _mountEntity.Skeleton.SetAgentActionChannel(0, act_inventory_idle_start, MBRandom.RandomFloat, -0.2f);
            }
            _mountEntity.EntityFlags |= EntityFlags.AnimateWhenVisible;
            MountVisualCreator.AddMountMeshToEntity(_mountEntity, _equipment[10].Item, _equipment[11].Item, _mountCreationKey, null);
            _mountEntity.SetFrame(ref _mountSpawnPoint);
        }

        private void RemoveMount()
        {
            bool flag = _mountEntity != null;
            if (flag)
            {
                _mountEntity.ClearComponents();
                _mountEntity.Remove(101);
                _mountEntity = null;
            }
        }

        internal void CharacterTableauContinuousRenderFunction(Texture sender, EventArgs e)
        {
            Scene scene = (Scene)sender.UserData;
            TableauView tableauView = sender.TableauView;
            bool flag = scene == null;
            if (flag)
            {
                tableauView.SetContinuousRendering(false);
                tableauView.SetDeleteAfterRendering(true);
            }
            else
            {
                scene.EnsurePostfxSystem();
                scene.SetDofMode(false);
                scene.SetMotionBlurMode(false);
                scene.SetBloom(true);
                scene.SetDynamicShadowmapCascadesRadiusMultiplier(0.31f);
                tableauView.SetRenderWithPostfx(true);
                float cameraRatio = _cameraRatio;
                MatrixFrame camPos = _camPos;
                bool flag2 = _continuousRenderCamera != null;
                if (flag2)
                {
                    Camera continuousRenderCamera = _continuousRenderCamera;
                    _continuousRenderCamera = null;
                    continuousRenderCamera.SetFovVertical(0.7853982f, cameraRatio, 0.2f, 200f);
                    continuousRenderCamera.Frame = camPos;
                    tableauView.SetCamera(continuousRenderCamera);
                    tableauView.SetScene(scene);
                    tableauView.SetSceneUsesSkybox(false);
                    tableauView.SetDeleteAfterRendering(false);
                    tableauView.SetContinuousRendering(true);
                    tableauView.SetDoNotRenderThisFrame(true);
                    tableauView.SetClearColor(0u);
                    tableauView.SetFocusedShadowmap(true, ref _frame.origin, 1.55f);
                }
            }
        }
    }
}
