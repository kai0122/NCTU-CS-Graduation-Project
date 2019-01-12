# NCTU-Graduation-Project
## Introduction: [PPT](https://www.emaze.com/@AOFLWFWRR/vr)
Our thesis is called: "Computer Vision-assisted virtual reality which reaches a wide range of multi-person simultaneous VR systems". We visually locate the virtual reality system, adding more possibilities to the interactive space of the virtual reality, enhancing fun and utility. By transmitting external camera lens signals to the host computer and using computer vision positioning system, we allow user to make multiple users interact in the same space, under the same system. We can get rid of the situation that only one person can experience the virtual world nowadays, making single space Multiple sets of VR devices being operated simultaneously to increase interactivity and entertainment. 

### Awards
This Project has won the Highest Distinction Award and even the 3rd place of the Popular Award in the competition of NCTU CS Project Competition.

### Demo Video for Preliminary: [Link](https://youtu.be/zEbyS8KqVNk)
### Demo Video for Final: [Link](https://youtu.be/CLhfdnUjIKY)
### Video for Final: [Link](https://youtu.be/lcc1GmF-_M0)

## 專題摘要
#### 關鍵詞：虛擬實境、電腦視覺、互動遊戲

### 專題研究動機與目的
現今市面上之VR遊戲多為單人在虛擬場景遊玩，並且必須手持HTC VIVE握把。因VIVE握把外型固定，而使得使用者體驗與虛擬場景內的景象不盡相同。而現今之多人VR雖然大都可讓玩家位於不同空間連線遊玩，沒有空間限制，但是玩家卻無法在現實場景摸到其他玩家，並與其他玩家互動，但是因為無法真正碰到其他玩家，而使得虛擬場景內與人互動的真實感不足。因此我們希望藉由我們的系統，提供玩家在同一空間、同一場景，在不需依靠VIVE握把的情況下體驗多人於虛擬場景的互動。
<img src="https://i.imgur.com/7nn7585.png" width="650" />


### 現有相關研究概況及比較
#### (一) VR與Kinect結合：W. Woodard, S. Sukittanon, "Interactive virtual building walkthrough using  oculus rift and microsoft kinect", Tech. Rep., Feb. 2013.
在關於將VR以及Kinect 結合部分，曾有研究將其應用在建築物內部結構體驗的研究上[4]，該研究將Oculus Rift以及Microsoft Kinect結合，人物可進入由Unity3D 建出的場景中走動，並利用Kinect 偵測手臂動作，角色的Model可以利用觸發場景中如門、吊燈等物件之collider，控制物件的開關。

#### (二) VR多人連線：
在VR多人連線的部分，[5] Sport Bar VR 是一款多人連線的VR遊戲，可以同時8人遊玩，玩家可在一間運動酒吧中與其他人互動玩撞球比賽射飛鏢等等，遊戲畫面並不會顯示整個玩家，而是標出頭盔和手把的部分，幫助玩家判斷彼此的位置。

目前市面上多人互動的VR遊戲大多以手把控制，較少加入Kinect，本專題則是將多人連線與Kinect動作偵測結合，使玩家能彼此看見不只手把部分的動作，而是整個身體動作，也能利用作出不同動作達成關卡任務或與對方互動，增加遊戲性。

### 專題重要貢獻
* 將第一人稱視角的跑酷遊戲移植到VR上面，更具震撼力。
* 使用Kinect模組連結到Unity遊戲中，將人體動作分析，以操縱畫面中人物，輔以VR，猶如真實場景。
* （未來預計）透過Unity的Network Manager模組將兩人的跑酷遊戲結合至同⼀場地，達成多人互動的效果（亦即：兩人可以⼀起合作闖關）。

### 設計原理、研究方法與步驟
#### (一) 設計原理：
透過電腦視覺的方式捕捉使用者動作，在VR場景中生成每位使用者的Avatar，並做出相應的動作使對方”看到”。由於兩個人都在同一空間，同時捕捉兩人的動作，因此相對距離、方向等都會與現實中相近，從而達成”在VR世界中與他人真實互動”的效果。

#### (二) 研究方法：
製作一款需要多人互動的VR遊戲，玩家之間要在物理上互動與合作才能達成遊戲目標，在我們的專題，玩家們要在一個賽道上向前跑，並做出特定動作以避開障礙物，其中有些動作是單人無法完成的，如手拉手等。藉此遊戲的體驗來測試我們的設計是否接近真實互動。

#### (三) 步驟：
1. 建立VR環境
2. 建立Unity場景
  * 遊戲場景
  * 人物模型
  * 障礙物
3. 編寫遊戲程式
  * 加入Kinect捕捉人體動作
  * 加入多人連線
  * 人物控制
  * 遊戲機制
  * UI
4. 測試遊戲體驗與多人互動效果
<img src="https://i.imgur.com/7Pe6Mlm.png" width="650" />

### 系統實現與實驗
#### (一) 3D建模：
我們使用MAYA與Blender進行雙人障礙物之3D建模，目前雙人之動作以同時跳起、蹲下、手拉手為建模參考動作。下左圖為透過MAYA經由裁切立方體而製作之雙人同時跳起之障礙物。中間圖為建立障礙物之Mesh，而右圖為已經建立好以雙人同時跳起為目標之障礙物。

<img src="https://i.imgur.com/JoB6wP2.png" width="650" />

#### (二) 連線系統：
我們使用Unity 多人連線管理工具，完成多人VR遊戲下，在不同電腦的網路資料傳輸與溝通(在同一個網域下，打對方的LAN IP) 或是於不同的網域下打對方的公眾固定IP)，使得自己的畫面看的見對方遊玩之動作，反之亦然，達成多人連線之功能。
<img src="https://i.imgur.com/037zcuv.png" width="650" />

#### (三) Kinect v2 Toolkit 控制角色模型：
我們運用Unity Asset Store找到讓人可以利⽤Kinect v2控制unity⼈物角⾊的資源都是下載並套用 [1] Kinect v2 Examples with MSSDK and Nuitrack SDK 這個Unity Asset Store的套件。這個套件是利⽤Kinect抓取並判斷使⽤者的骨架，將其對應到Unity 裡⾯的角色骨架，進⽽達到控制角⾊的目的，如右圖。 人物模型的部分，我們採用的是Unity官方推出的Unity Chan [3]角色模型，其中已經包含完整骨架以及Mesh的建立。
此Toolkit包含四個主要的Demo Scene，我們取第四個Demo Scene套⽤在互動應⽤上，使得只有在有人出現在Kinect鏡頭前面時，才會出現角色Model。
這個Toolkit在遊戲裡的應用是利用玩家透過套件本來就可以控制角色這件事，讓玩家透過控制跑酷遊戲裡面角色的局部身體移動，包含四隻與腰部等等。並利用玩家左手是否握拳來決定玩家前進與否。
<img src="https://i.imgur.com/GtYyKoO.png" height="300" />

#### (四) 遊戲環境建立：
我們透過購買Unity Asset Store已建立好的Temple Run遊戲的森林背景 [2] Ultimate Runner Engine，省下自己建立背景的時間，以完成其他系統架構。然而我們並沒有使用其遊戲架構的程式碼，而是採用他們建立好的場景與跑道，例如樹木、猴子與火車...等美術模型。透過拆解我們需要的場景模型，加入到我們的遊戲之中，以建立我們的VR場景。然而若之後時間允許，我們會自己建立場景並將其替換為以我們遊戲主題建立的場景模型。
<img src="https://i.imgur.com/vB5qOjj.jpg" width="650" />

#### (五) VR場景建立與架設：
我們將Steam VR匯入到Unity中，並將Camera Rig以及Steam VR加入到Unity場景內，其中Camera Rig包含Camera Eye以及VIVE握把的資訊。由於我們的動機是希望玩家不需要使用VIVE握把，因此上述資訊我們並不會使用到。對於玩家在Unity場景內的向前移動、向上跳起，我們藉由控制Camera Rig的Position來達成。我們將角色綁定在Camera Rig中，並將Camera Head之位置綁定在角色眼睛部位。對於玩家本身左右前後在真實場景小範圍內的移動，我們以HTC Vive Lighthouse捕捉角色在VR場景內遊玩跑酷遊戲之左右前後移動，轉換成Camera Eye在Camera Rig中的移動來控制角色。而玩家身體、四肢的移動，我們則透過Kinect v2捕捉，並利用Kinect v2 Toolkit來控制Unity場景內的Avatar，作為多人互動遊戲玩家的局部身體移動。
##### VR顯示User Interface建立步驟：
1. 將canvas 調整成可以適用在VR頭盔上。
2. 將頭盔的canvas render 調成Screen Space - Camera。
3. 將VR的head camera 加入canvas的camera field。
玩家移動對應圖：
<img src="https://i.imgur.com/olEWqEe.png" width="400" />


#### 硬體需求：
* Xbox Kinect v2
* HTC Vive * 2
* HTC Vive Lighthouse
* 兩台電腦
* 硬體配置簡圖如下：
<img src="https://i.imgur.com/Xrv5IgB.png" width="450" />


### 參考文獻
[1] 	Kinect v2 Examples with MS-SDK and Nuitrack SDK
https://assetstore.unity.com/packages/3d/characters/kinect-v2-examples-withms-
sdk-and-nuitrack-sdk-18708

[2] 	Ultimate Runner Engine
https://assetstore.unity.com/packages/templates/systems/ultimate-runner-engine-77843

[3] Unity Chan
http://unity-chan.com/
https://assetstore.unity.com/packages/3d/characters/unity-chan-model-18705

[4] W. Woodard, S. Sukittanon, "Interactive virtual building walkthrough using  oculus rift and microsoft kinect", Tech. Rep., Feb. 2013.
https://www.utm.edu/webshare/cens/papers/19120744-IEEE%202015%20Oculus%20ver%204.1.pdf

[5] Sports Bar VR
https://store.steampowered.com/app/269170/Sports_Bar_VR/

[6] 3D 建模：Maya: How to trim NURBS surfaces | lynda.com tutorial
https://www.youtube.com/watch?v=X2blYvA4NBY&fbclid=IwAR3wFR52AI_RCiRnW6IY-4l09BvL1nJu7vo6_5DYh4GrJ_t8XQhUb6FCG7E
使用Maya surface 工具繪製curve，實做會使用到弧面之模型，引用其中技術進行障礙物的建模。

[7] 遊戲設計：Unity 5 - Type Writer Effect Tutorial (Great for Dialogues)
https://www.youtube.com/watch?v=1qbjmb_1hV4
特殊的文字設計方式，用於顯示UI之遊戲訊息

[8] 網路連線：Unity – Using UNET making a multiplayer game
https://www.youtube.com/watch?v=Ba32hdZ4QdY&feature=share&fbclid=IwAR2G1knU3ltMEbEeP9oyS-SDqaZkOLPXnfcEh2WTrO5kO5ojE6r6stuLlRs
使用Unity 的 網路工具進行多人連線fps遊戲製作，使用其技術將玩家所屬的畫面傳給對方。

[9] VR：User Interfaces for VR
https://unity3d.com/learn/tutorials/topics/virtual-reality/user-interfaces-vr
介紹各種VR玩家互動介面，如 Non-diegetic、Spatial-UI…等等，其中包含將Canvas 調整至VR模式之教學。
