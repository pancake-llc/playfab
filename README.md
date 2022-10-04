# How To Install

Add the lines below to Packages/manifest.json

- for version 2.144.220804
```csharp
"com.pancake.gameservice": "https://github.com/pancake-llc/game-service.git?path=Assets/_Root#2.144.220804",
"com.lupidan.apple-signin-unity": "https://github.com/lupidan/apple-signin-unity.git#v1.4.2",
"com.coffee.ui-effect": "https://github.com/mob-sakai/UIEffect.git#4.0.0-preview.9",
"com.coffee.ui-particle": "https://github.com/mob-sakai/ParticleEffectForUGUI.git#4.1.6",
```

Dependency : Facebook

````csharp
"com.pancake.facebook": "https://github.com/pancake-llc/facebook.git?path=Assets/_Root#1.0.7",
"com.google.external-dependency-manager": "https://github.com/pancake-llc/external-dependency-manager.git?path=Assets/_Root#1.2.170",
````

Dependency : Heart
```csharp
"com.pancake.heart": "https://github.com/pancake-llc/heart.git?path=Assets/_Root",
```


![playfab-anonymous-login-and-recoverable-login-min](https://user-images.githubusercontent.com/44673303/166100604-75c5949d-8c71-4b67-abbc-eb752ec51bfa.png)

![compact_login](https://user-images.githubusercontent.com/44673303/166114223-13fb92e7-00cc-4947-b33f-50f54acf2270.png)


# Usages

- for update score to leaderboard when first time you enter name complete. You can via using `valueExpression` in `ButtonLeaderBoard`

```c#
GetComponent<ButtonLeaderboard>().valueExpression += () => UnityEngine.Random.Range(1, 100);
```