// GENERATED AUTOMATICALLY FROM 'Assets/Scripts/PlayerControls.inputactions'

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;

public class @PlayerControls : IInputActionCollection, IDisposable
{
    public InputActionAsset asset { get; }
    public @PlayerControls()
    {
        asset = InputActionAsset.FromJson(@"{
    ""name"": ""PlayerControls"",
    ""maps"": [
        {
            ""name"": ""Player"",
            ""id"": ""93cc3b3d-d020-4d8b-8598-1210bd1d2cbd"",
            ""actions"": [
                {
                    ""name"": ""PlayerHorizontalMovement"",
                    ""type"": ""Button"",
                    ""id"": ""c92e9921-9258-44bb-9c5e-31f4d2e5ad86"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""PlayerVerticalMovement"",
                    ""type"": ""Button"",
                    ""id"": ""bf37cbfa-d718-4220-934c-3cb061e33120"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""PlayerJump"",
                    ""type"": ""PassThrough"",
                    ""id"": ""324aa8c8-a35d-460c-86e8-f42d4e05989f"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""PlayerAttack"",
                    ""type"": ""PassThrough"",
                    ""id"": ""9b058838-79e4-41f6-a9e7-09b80d8dca4b"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Select"",
                    ""type"": ""PassThrough"",
                    ""id"": ""7823bfde-1efd-412a-9ca8-2cd331ab4376"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Player4AxisMovement"",
                    ""type"": ""Button"",
                    ""id"": ""850ecee0-14a6-412a-9dac-47dc401a26d8"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Back"",
                    ""type"": ""Button"",
                    ""id"": ""9acadd10-76bb-4724-a857-5483c52fe8f1"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""PlayerCrouch"",
                    ""type"": ""PassThrough"",
                    ""id"": ""4c9594db-488f-478b-ab9a-919d62ab276c"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""4fb20f9f-fb82-4a24-adba-9390df38db00"",
                    ""path"": ""<Keyboard>/y"",
                    ""interactions"": ""Press"",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""PlayerJump"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""16ce5fc5-50d8-42cf-bd56-57dd8211c6ac"",
                    ""path"": ""<Keyboard>/numpad5"",
                    ""interactions"": ""Press"",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""PlayerJump"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""5bd980bd-5428-4e0f-aacd-bc4ff8ed9355"",
                    ""path"": ""<Gamepad>/buttonNorth"",
                    ""interactions"": ""Press"",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""PlayerJump"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""2a700504-1639-49dc-9388-a3f6614c97ee"",
                    ""path"": ""<Gamepad>/dpad/up"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""PlayerJump"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""7c216c27-172f-4167-8ff9-c834f034c28a"",
                    ""path"": ""<Keyboard>/g"",
                    ""interactions"": ""Press"",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""PlayerAttack"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""bee72358-6705-42a9-8483-c33234595be0"",
                    ""path"": ""<Keyboard>/numpad1"",
                    ""interactions"": ""Press"",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""PlayerAttack"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""3adb8bba-1535-4449-b17e-e1b0a5b45129"",
                    ""path"": ""<Gamepad>/buttonWest"",
                    ""interactions"": ""Press"",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""PlayerAttack"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""01dfafb1-b5c4-40a2-b23c-1060e1376b28"",
                    ""path"": ""<DualShockGamepad>/buttonWest"",
                    ""interactions"": ""Press"",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""PlayerAttack"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""d5ffee3d-2f0d-4843-a41d-eb1705f24d46"",
                    ""path"": ""<Gamepad>/buttonSouth"",
                    ""interactions"": ""Press"",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Select"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""9066b26c-6884-4615-a790-808c77f0ecbd"",
                    ""path"": ""<Keyboard>/g"",
                    ""interactions"": ""Press"",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Select"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""ed602df4-50a8-4994-b0af-1c68a5fd475d"",
                    ""path"": ""<Keyboard>/numpad1"",
                    ""interactions"": ""Press"",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Select"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""VerticalMovement(WASD)"",
                    ""id"": ""ced57ec5-954a-4083-aba2-b22bb5759380"",
                    ""path"": ""1DAxis"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""PlayerVerticalMovement"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""negative"",
                    ""id"": ""07dc6f3b-4b24-430f-808c-acaa66fc792b"",
                    ""path"": ""<Keyboard>/s"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""PlayerVerticalMovement"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""positive"",
                    ""id"": ""4809f319-9229-4efd-9bde-fc4e69dc8cf1"",
                    ""path"": ""<Keyboard>/w"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""PlayerVerticalMovement"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""VerticalMovement(ARROWS)"",
                    ""id"": ""d51b353f-13ae-4d75-9316-cb263884eda0"",
                    ""path"": ""1DAxis"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""PlayerVerticalMovement"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""negative"",
                    ""id"": ""a9583c71-1ce1-4481-bb3d-c74ba993b828"",
                    ""path"": ""<Keyboard>/downArrow"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""PlayerVerticalMovement"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""positive"",
                    ""id"": ""49b68be9-5a25-42e9-b697-094195ca9737"",
                    ""path"": ""<Keyboard>/upArrow"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""PlayerVerticalMovement"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""VerticalMovement(Gamepad)"",
                    ""id"": ""00728e63-9215-4b40-b674-c8417bcee012"",
                    ""path"": ""1DAxis"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""PlayerVerticalMovement"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""negative"",
                    ""id"": ""5208d9c2-8a42-47a6-ade5-97c2c59db43a"",
                    ""path"": ""<Gamepad>/leftStick/down"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""PlayerVerticalMovement"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""positive"",
                    ""id"": ""2a7becb0-2157-434d-a7d0-30d14a105981"",
                    ""path"": ""<Gamepad>/leftStick/up"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""PlayerVerticalMovement"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""HorizontalMovement(WASD)"",
                    ""id"": ""2106ed58-2342-4d6e-b9cb-033c7f2dbcbb"",
                    ""path"": ""1DAxis"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""PlayerHorizontalMovement"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""negative"",
                    ""id"": ""a2c11e86-f6a3-46d5-949b-408e840b8606"",
                    ""path"": ""<Keyboard>/a"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""PlayerHorizontalMovement"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""positive"",
                    ""id"": ""06a815e4-990c-441c-9e9f-b1138f5949a1"",
                    ""path"": ""<Keyboard>/d"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""PlayerHorizontalMovement"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""HorizontalMovement(ARROWS)"",
                    ""id"": ""3d514c97-df0a-41fd-ba90-1bd836e328c9"",
                    ""path"": ""1DAxis"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""PlayerHorizontalMovement"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""negative"",
                    ""id"": ""d4659211-4577-4f76-8644-fab6453ce9f4"",
                    ""path"": ""<Keyboard>/leftArrow"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""PlayerHorizontalMovement"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""positive"",
                    ""id"": ""bc63f4db-2a3e-427e-aa5b-e5f742a340fb"",
                    ""path"": ""<Keyboard>/rightArrow"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""PlayerHorizontalMovement"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""HorizontalMovement(Gamepad)"",
                    ""id"": ""c858f84e-5a0e-4c82-8000-4b58781e0636"",
                    ""path"": ""1DAxis"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""PlayerHorizontalMovement"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""negative"",
                    ""id"": ""5f431d52-3b99-43ab-b1a2-ecd95da46a2a"",
                    ""path"": ""<Gamepad>/leftStick/left"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""PlayerHorizontalMovement"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""positive"",
                    ""id"": ""2f0eff71-5194-4d75-a5ff-0e0573504ea7"",
                    ""path"": ""<Gamepad>/leftStick/right"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""PlayerHorizontalMovement"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""WASD"",
                    ""id"": ""b1dc1fa9-9e0b-4b23-8528-50783fb8c692"",
                    ""path"": ""2DVector"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Player4AxisMovement"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""up"",
                    ""id"": ""f79dff00-0256-4c8e-8df4-afa3e20f5d02"",
                    ""path"": ""<Keyboard>/w"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Player4AxisMovement"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""down"",
                    ""id"": ""901ed5c8-10ee-46e8-93b0-f27702960ecb"",
                    ""path"": ""<Keyboard>/s"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Player4AxisMovement"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""left"",
                    ""id"": ""7410521e-d10c-4b15-8da7-bf73819d4e7d"",
                    ""path"": ""<Keyboard>/a"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Player4AxisMovement"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""right"",
                    ""id"": ""f8513dfb-f3e3-4771-818f-ad150d27a73a"",
                    ""path"": ""<Keyboard>/d"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Player4AxisMovement"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""ARROWS"",
                    ""id"": ""2ef82ec4-50cf-46bf-a17c-ef76fda41e5d"",
                    ""path"": ""2DVector"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Player4AxisMovement"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""up"",
                    ""id"": ""b63bfab1-ac4d-4bd9-a2bf-aa4200381384"",
                    ""path"": ""<Keyboard>/upArrow"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Player4AxisMovement"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""down"",
                    ""id"": ""8a91204c-4ae0-4413-ba5a-fae350c41006"",
                    ""path"": ""<Keyboard>/downArrow"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Player4AxisMovement"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""left"",
                    ""id"": ""70911eac-c4a9-4c19-a589-a759038e66d5"",
                    ""path"": ""<Keyboard>/leftArrow"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Player4AxisMovement"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""right"",
                    ""id"": ""ef9faa18-d78e-4b6b-9d7e-c6c225a058c8"",
                    ""path"": ""<Keyboard>/rightArrow"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Player4AxisMovement"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""LeftJoystick(Gamepad)"",
                    ""id"": ""0cfe15eb-6eb1-4e3d-bb67-aeb3188cddda"",
                    ""path"": ""2DVector"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Player4AxisMovement"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""up"",
                    ""id"": ""262c7a85-6659-4901-a063-2dda1e89f194"",
                    ""path"": ""<Gamepad>/leftStick/up"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Player4AxisMovement"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""down"",
                    ""id"": ""471f90df-aac4-4359-993b-7265ed7f3844"",
                    ""path"": ""<Gamepad>/leftStick/down"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Player4AxisMovement"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""left"",
                    ""id"": ""24e307c0-aaf1-460d-abde-1f8052154290"",
                    ""path"": ""<Gamepad>/leftStick/left"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Player4AxisMovement"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""right"",
                    ""id"": ""920c877d-b7d6-471e-9a66-faf69c13bb72"",
                    ""path"": ""<Gamepad>/leftStick/right"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Player4AxisMovement"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": """",
                    ""id"": ""bb1b83bf-b5ec-453e-9c5d-cd571528198b"",
                    ""path"": ""<Keyboard>/h"",
                    ""interactions"": ""Press"",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Back"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""b3c2844d-7105-44a5-80db-91628cc4edae"",
                    ""path"": ""<Keyboard>/s"",
                    ""interactions"": ""Hold"",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""PlayerCrouch"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        }
    ],
    ""controlSchemes"": []
}");
        // Player
        m_Player = asset.FindActionMap("Player", throwIfNotFound: true);
        m_Player_PlayerHorizontalMovement = m_Player.FindAction("PlayerHorizontalMovement", throwIfNotFound: true);
        m_Player_PlayerVerticalMovement = m_Player.FindAction("PlayerVerticalMovement", throwIfNotFound: true);
        m_Player_PlayerJump = m_Player.FindAction("PlayerJump", throwIfNotFound: true);
        m_Player_PlayerAttack = m_Player.FindAction("PlayerAttack", throwIfNotFound: true);
        m_Player_Select = m_Player.FindAction("Select", throwIfNotFound: true);
        m_Player_Player4AxisMovement = m_Player.FindAction("Player4AxisMovement", throwIfNotFound: true);
        m_Player_Back = m_Player.FindAction("Back", throwIfNotFound: true);
        m_Player_PlayerCrouch = m_Player.FindAction("PlayerCrouch", throwIfNotFound: true);
    }

    public void Dispose()
    {
        UnityEngine.Object.Destroy(asset);
    }

    public InputBinding? bindingMask
    {
        get => asset.bindingMask;
        set => asset.bindingMask = value;
    }

    public ReadOnlyArray<InputDevice>? devices
    {
        get => asset.devices;
        set => asset.devices = value;
    }

    public ReadOnlyArray<InputControlScheme> controlSchemes => asset.controlSchemes;

    public bool Contains(InputAction action)
    {
        return asset.Contains(action);
    }

    public IEnumerator<InputAction> GetEnumerator()
    {
        return asset.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }

    public void Enable()
    {
        asset.Enable();
    }

    public void Disable()
    {
        asset.Disable();
    }

    // Player
    private readonly InputActionMap m_Player;
    private IPlayerActions m_PlayerActionsCallbackInterface;
    private readonly InputAction m_Player_PlayerHorizontalMovement;
    private readonly InputAction m_Player_PlayerVerticalMovement;
    private readonly InputAction m_Player_PlayerJump;
    private readonly InputAction m_Player_PlayerAttack;
    private readonly InputAction m_Player_Select;
    private readonly InputAction m_Player_Player4AxisMovement;
    private readonly InputAction m_Player_Back;
    private readonly InputAction m_Player_PlayerCrouch;
    public struct PlayerActions
    {
        private @PlayerControls m_Wrapper;
        public PlayerActions(@PlayerControls wrapper) { m_Wrapper = wrapper; }
        public InputAction @PlayerHorizontalMovement => m_Wrapper.m_Player_PlayerHorizontalMovement;
        public InputAction @PlayerVerticalMovement => m_Wrapper.m_Player_PlayerVerticalMovement;
        public InputAction @PlayerJump => m_Wrapper.m_Player_PlayerJump;
        public InputAction @PlayerAttack => m_Wrapper.m_Player_PlayerAttack;
        public InputAction @Select => m_Wrapper.m_Player_Select;
        public InputAction @Player4AxisMovement => m_Wrapper.m_Player_Player4AxisMovement;
        public InputAction @Back => m_Wrapper.m_Player_Back;
        public InputAction @PlayerCrouch => m_Wrapper.m_Player_PlayerCrouch;
        public InputActionMap Get() { return m_Wrapper.m_Player; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(PlayerActions set) { return set.Get(); }
        public void SetCallbacks(IPlayerActions instance)
        {
            if (m_Wrapper.m_PlayerActionsCallbackInterface != null)
            {
                @PlayerHorizontalMovement.started -= m_Wrapper.m_PlayerActionsCallbackInterface.OnPlayerHorizontalMovement;
                @PlayerHorizontalMovement.performed -= m_Wrapper.m_PlayerActionsCallbackInterface.OnPlayerHorizontalMovement;
                @PlayerHorizontalMovement.canceled -= m_Wrapper.m_PlayerActionsCallbackInterface.OnPlayerHorizontalMovement;
                @PlayerVerticalMovement.started -= m_Wrapper.m_PlayerActionsCallbackInterface.OnPlayerVerticalMovement;
                @PlayerVerticalMovement.performed -= m_Wrapper.m_PlayerActionsCallbackInterface.OnPlayerVerticalMovement;
                @PlayerVerticalMovement.canceled -= m_Wrapper.m_PlayerActionsCallbackInterface.OnPlayerVerticalMovement;
                @PlayerJump.started -= m_Wrapper.m_PlayerActionsCallbackInterface.OnPlayerJump;
                @PlayerJump.performed -= m_Wrapper.m_PlayerActionsCallbackInterface.OnPlayerJump;
                @PlayerJump.canceled -= m_Wrapper.m_PlayerActionsCallbackInterface.OnPlayerJump;
                @PlayerAttack.started -= m_Wrapper.m_PlayerActionsCallbackInterface.OnPlayerAttack;
                @PlayerAttack.performed -= m_Wrapper.m_PlayerActionsCallbackInterface.OnPlayerAttack;
                @PlayerAttack.canceled -= m_Wrapper.m_PlayerActionsCallbackInterface.OnPlayerAttack;
                @Select.started -= m_Wrapper.m_PlayerActionsCallbackInterface.OnSelect;
                @Select.performed -= m_Wrapper.m_PlayerActionsCallbackInterface.OnSelect;
                @Select.canceled -= m_Wrapper.m_PlayerActionsCallbackInterface.OnSelect;
                @Player4AxisMovement.started -= m_Wrapper.m_PlayerActionsCallbackInterface.OnPlayer4AxisMovement;
                @Player4AxisMovement.performed -= m_Wrapper.m_PlayerActionsCallbackInterface.OnPlayer4AxisMovement;
                @Player4AxisMovement.canceled -= m_Wrapper.m_PlayerActionsCallbackInterface.OnPlayer4AxisMovement;
                @Back.started -= m_Wrapper.m_PlayerActionsCallbackInterface.OnBack;
                @Back.performed -= m_Wrapper.m_PlayerActionsCallbackInterface.OnBack;
                @Back.canceled -= m_Wrapper.m_PlayerActionsCallbackInterface.OnBack;
                @PlayerCrouch.started -= m_Wrapper.m_PlayerActionsCallbackInterface.OnPlayerCrouch;
                @PlayerCrouch.performed -= m_Wrapper.m_PlayerActionsCallbackInterface.OnPlayerCrouch;
                @PlayerCrouch.canceled -= m_Wrapper.m_PlayerActionsCallbackInterface.OnPlayerCrouch;
            }
            m_Wrapper.m_PlayerActionsCallbackInterface = instance;
            if (instance != null)
            {
                @PlayerHorizontalMovement.started += instance.OnPlayerHorizontalMovement;
                @PlayerHorizontalMovement.performed += instance.OnPlayerHorizontalMovement;
                @PlayerHorizontalMovement.canceled += instance.OnPlayerHorizontalMovement;
                @PlayerVerticalMovement.started += instance.OnPlayerVerticalMovement;
                @PlayerVerticalMovement.performed += instance.OnPlayerVerticalMovement;
                @PlayerVerticalMovement.canceled += instance.OnPlayerVerticalMovement;
                @PlayerJump.started += instance.OnPlayerJump;
                @PlayerJump.performed += instance.OnPlayerJump;
                @PlayerJump.canceled += instance.OnPlayerJump;
                @PlayerAttack.started += instance.OnPlayerAttack;
                @PlayerAttack.performed += instance.OnPlayerAttack;
                @PlayerAttack.canceled += instance.OnPlayerAttack;
                @Select.started += instance.OnSelect;
                @Select.performed += instance.OnSelect;
                @Select.canceled += instance.OnSelect;
                @Player4AxisMovement.started += instance.OnPlayer4AxisMovement;
                @Player4AxisMovement.performed += instance.OnPlayer4AxisMovement;
                @Player4AxisMovement.canceled += instance.OnPlayer4AxisMovement;
                @Back.started += instance.OnBack;
                @Back.performed += instance.OnBack;
                @Back.canceled += instance.OnBack;
                @PlayerCrouch.started += instance.OnPlayerCrouch;
                @PlayerCrouch.performed += instance.OnPlayerCrouch;
                @PlayerCrouch.canceled += instance.OnPlayerCrouch;
            }
        }
    }
    public PlayerActions @Player => new PlayerActions(this);
    public interface IPlayerActions
    {
        void OnPlayerHorizontalMovement(InputAction.CallbackContext context);
        void OnPlayerVerticalMovement(InputAction.CallbackContext context);
        void OnPlayerJump(InputAction.CallbackContext context);
        void OnPlayerAttack(InputAction.CallbackContext context);
        void OnSelect(InputAction.CallbackContext context);
        void OnPlayer4AxisMovement(InputAction.CallbackContext context);
        void OnBack(InputAction.CallbackContext context);
        void OnPlayerCrouch(InputAction.CallbackContext context);
    }
}
