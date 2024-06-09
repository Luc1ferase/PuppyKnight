# PuppyKnight

游戏引擎开发课-3D项目学习

### Tips:

vs多行注释：CTRL+K,CTRL +C  
取消注释： CTRL+K,CTRL+U

### BUG:动画Read-Only，无法插入事件时

//图片待补充
![image](Puppy Knight\ReadmePics\read-only.png "123")

Ctrl + D 复制一个出来，跳出fbx文件就可以编辑了

#### Lesson12--FoundPlayer 找到Player追击

New Features:

--Cinemachine FreeLook Camera

--实现滚轮和AD键切换视角

#### Lesson18

New Features:

--添加了guard模式和对死亡的判定  
--添加了史莱姆守卫模式和死亡的动画

#### Lesson19

New Features:

--Singleton and interface study

#### Lesson20

New Features:

--Observer Pattern Interface Implementation Observer Pattern Subscription.

Bug Fixes:  
--lesson19中未在unity中创建GameManager对象，导致获取游戏对象时抛了空指针异常

TODO:  
--Add more enemies.

#### Lesson21

New Features:

--Now we have more Enemies,Grunt,Golem.  
--Added generic attribute script by Override it to allow us to quickly create an enemy

TODO:

Setup more Grunts.

#### Lesson22

New Features:

--Player Dizzy animation when Attacked by Enemy.
--Animation state machine:Stop NavMeshAgent when Dizzy animation is triggered

Bug Fixes:

--Modified the Grunt's skill trigger distance so that it now triggers the knockback animation properly.

--Lesson21 中的

EnemyController.cs : 77 条件判断错误

```csharp
void OnDisable()
    {
        if (GameManager.IsInitialized)
            return;
        GameManager.Instance.RemoveObserver(this);
    }
```

=>

```csharp
void OnDisable()
    {
        if (!GameManager.IsInitialized) //Modified this line
            return;
        GameManager.Instance.RemoveObserver(this);
    }
```

#### Lesson23

Tips:
如果出现动画滑动的情况，取消NavMeshAgent中的Auto Braking

New Features:  
--Add Extension Method script which can judge if the attack is facing target.

TODO:  
--Setup Golem

#### Lesson24

Tips:

```csharp
Vector3 direction = (attackTarget.transform.position - transform.position).normalized;
```

==

```csharp
Vector3 direction = attackTarget.transform.position - transform.position;
    direction.Normalize();
```

New Features:

--Add up new Enemy Golem which has Powerful Knockout attack.

#### Lesson25

Bug Fixes:  
//CharacterStats.cs : 85 暴击逻辑判断修改

```csharp
if (isCritical)
        {
            defender.GetComponent<Animator>().SetTrigger("Hit");
        }
```

=>

```csharp
if (attacker.isCritical) //Modified this line
        {
            defender.GetComponent<Animator>().SetTrigger("Hit");
        }
```

New Features:  

Mesh Collider作用于石头人投出的石头，可以完全贴合素材

#### Lesson26


New Features:  
--反击石头时的粒子特效

Tips:
```csharp
rb.velocity = Vector3.one
```
==
```
rb.velocity = new Vector3(1, 1, 1);
```

人物的Rigidbody勾选is Kinematic=>防止与NavMeshAgent冲突