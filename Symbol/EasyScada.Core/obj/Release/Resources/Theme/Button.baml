   M S B A M L   `   `   `  ���� \  XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35N;clr-namespace:System.Windows;assembly=PresentationFrameworkSystem.Windows  W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35^ ZWindowsFormsIntegration, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089  �� MergedDictionaries �� Source   NuclearButtonFocusVisual �� Property �� Value �� StrokeDashArray  Office2010SilverButtonStyle �� 
TargetType �� BasedOn % !Office2010SilverButtonNormalBrush + 'Office2010SilverButtonNormalBorderBrush �� 
TargetType � 	Resources  Key	 �� 
TargetName
 �� TargetProperty	 W� To g� Duration g� 	BeginTime R� KeyTime   System.Windows.Visibility    Visible R� Value O� KeyTime O� Value �Name ��Name �� CornerRadius . *Office2010SilverButtonNormalHighlightBrush J� Opacity ( $Office2010SilverButtonMouseOverBrush . *Office2010SilverButtonMouseOverBorderBrush & "Office2010SilverButtonPressedBrush ,	 (Office2010SilverButtonPressedBorderBrush /
 +Office2010SilverButtonPressedHighlightBrush 2 .Office2010SilverButtonPressedShadowBorderBrush 7 3Office2010SilverButtonPressedInnerShadowBorderBrush 1 -Office2010SilverButtonDisabledBackgroundBrush - )Office2010SilverButtonDisabledBorderBrush J� IsHitTestVisible ��Name ��Name K� ZIndex  ButtonFocusBrush �� Triggers P� Property P� Value �� 
TargetName 0 ,VIBlendOffice2010SilverButtonForegroundBrush N� EnterActions  ��Name  	FocusedOn! N� ExitActions  
FocusedOff  HoverOff  HoverOn C ?VIBlendOffice2010SilverButtonHighlightAndPressedForegroundBrush  
PressedOff  	PressedOn 8 4VIBlendOffice2010SilverButtonDisabledForegroundBrush�� F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation      :x,http://schemas.microsoft.com/winfx/2006/xaml      Cvsm;clr-namespace:System.Windows;assembly=PresentationFramework    �� $2 ,/EasyScada.Core;component/Theme/Generic.xaml?�%?  &	        &	 Y     7   7  7  7  7  7  7  7 	 7 
 7  7  7  7  7  7  7  7  7  7  7  7  7  7  7  7  7  7  7  �� .���  � �� �� .R��� ._��� $��2q����	#60000000$	 1 2R�$��1���� " �� Z� .���  �@�FocusVisualStyle8   ��  � ��
 �   ���  � ��$ Segoe UI)���  � ��$ 12(���  � ��$ 3q���  � ��8  ��  � ��8  ��  � �� �� " �� ���� HoverOn .��W� $	 BackgroundOver��$
 Opacity�$ 1X�$ 00:00:00.1000000D�W� $	 BackgroundOver_Highlight��$
 Opacity�$	 1.0X�$ 00:00:00.1000000D��� HoverOff .��W� $	 BackgroundOver��$
 Opacity�$ 0X�$ 00:00:00.2000000D�W� $	 BackgroundOver_Highlight��$
 Opacity�$ 0X�$ 00:00:00.2000000D��� 	PressedOn .��W� $	 BackgroundPressed��$
 Opacity�$
 0.84X�$ 00:00:00.1000000D�W� $ 	 BackgoundPressed_Highlight��$
 Opacity�$	 1.0X�$ 00:00:00.1000000D�W� $	 BackgroundPressedShadow��$
 Opacity�$
 0.84X�$ 00:00:00.1000000D�W� $"	 BackgroundPressedInnerShadow��$
 Opacity�$	 1.0X�$ 00:00:00.1000000D��� 
PressedOff .��W� $	 BackgroundPressed��$
 Opacity�$ 0X�$ 00:00:00.2000000D�W� $ 	 BackgoundPressed_Highlight��$
 Opacity�$ 0X�$ 00:00:00.2000000D�W� $	 BackgroundPressedShadow��$
 Opacity�$	 0.0X�$ 00:00:00.2000000D�W� $"	 BackgroundPressedInnerShadow��$
 Opacity�$	 0.0X�$ 00:00:00.2000000D��� 
DisabledOn .��T� $ 00:00:00X�$	 DisabledVisualElement��$
 (UIElement.Visibility)�..�i� $ 00:00:00.1000000��# Z T� $ 00:00:00X�$	 DisabledOverlay��$
 (UIElement.Visibility)�..�i� $ 00:00:00.1000000��# Z �� 	FocusedOn .��U� $ 00:00:00X�$	 FocusVisualElement��$
 (UIElement.Opacity)�.\��� $ 00:00:00.1000000��$ 1Q��� 
FocusedOff .��U� $ 00:00:00X�$	 FocusVisualElement��$
 (UIElement.Opacity)�.\��� $ 00:00:00.3000000��$ 0Q�.R�� $
 Grid��.+��� $ BackgroundNorm��#��z��#��z��$��1q�$
 1.75���� $ BackgroundNorm_highlight��$��1q�8�� $��1,1,1,1q�$ 1��$
 0.65Q��� $ BackgroundOver��8�� 8�� $��1q�$
 1.75��$ 0Q��� $ BackgroundOver_Highlight��$��1q����	#00FFFFFF$��1,1,1,1q�$ 1��$ 0Q��� $ BackgroundPressed��8�� 8�� $��1q�$
 1.75��$ 0Q��� $  BackgoundPressed_Highlight��$��1q�8�� $��1,1,1,1q�$ 1��$ 0Q��� $ BackgroundPressedShadow��$��1q�
������ 8��	 $��1q�$
 1.75��$ 0Q��� $" BackgroundPressedInnerShadow��$��2q�
������ 8��
 $��1q�$
 1.75��$ 0Q��� $ DisabledVisualElement��8�� 8�� $��1q� .  $ 0Q��� $ contentPresenter��#��z��#��z��#��z��#��z��#��z���� $ DisabledOverlay��#��z��#��z��#��z��$ 1�����	#FF8E96A2#��z��$y�	Collapsed=��� $ FocusVisualElement��$��-1q�8�� $��1q�$
 2.75�� .  $ 0Q��� $ DefaultBorder��$��-1q�8�� $��1q�$
 2.75�� .  $ 0Q� P�  �@��	IsDefault . . ��� $ DefaultBorder�� �@J�Opacity$ 1Q��� 8   � ��P�  �@J�	IsFocused .  �� $  FocusedOn_BeginStoryboard��8�� ! �� $   FocusedOff_BeginStoryboard��8�� P�  �@J�IsKeyboardFocused . ! �� $!  FocusedOff_BeginStoryboard1��8��  �� $   FocusedOn_BeginStoryboard1��8�� P�  �@J�IsMouseOver . ! �� $  HoverOff_BeginStoryboard��8��  �� 8�� . ��� 8   � ��P�  � �� . ! �� 8��  �� 8�� . ��� 8   � ��P�  � {� . . ��� 8   � ��P�  � {� .  . ��� 8   � ��