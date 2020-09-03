
using UnityEngine;
using System.Collections;
using UnityEngine.Audio;
using Extensions.UnityEngine;
using System;

[Prefab("SoundManager", true)]
public class SoundManager : Singleton<SoundManager> {



    [SerializeField]
    private AudioMixer audioMixer;


    [SerializeField]
    private AudioClip selectionLevelMusic, gameMenuMusic, selectionShipMusic, introMusicGame, musicGame, winMusic, loseMusic, shopMusic;

    [SerializeField]
    private AudioClip neutralizerRayShoot, expansiveWave, shipMoving, playerHit, neutralizerSoundHitEnemyShip;

    [SerializeField]
    private AudioClip naveEnemyShotSound, garbageDestroyed;

    [SerializeField]
    private AudioClip clickAlternative, click;




    private bool soundMaster, soundEffects, soundMusic;


    private float volumeMausicStart, volumeEffectsStart;

    private AudioSource audioSourceClick,
        audioSourceintroMusicGame, audioSourceSelectionLevelMusicGame, audioSourceGameMenuMusic, audioSourceSelectionShipMusicGame, audioSourceClickAlternative, audioSourceStoreMusic,
        audioSourceNeutralizerRayEffect, audioSourceExpanaiveWaveEffect, audioSourceEnemyShootEffect, audioSourceShipPlayerMoving, audioSourcePlayerHit, audioSourcePlayerShipExploding, audioSourceGarbageDefeated,
        audioSourceLoseMusic, audioSourceWinMusic,
       neutralizerSoundWhenRayHitShip;


    public AudioMixer AudioMixer {
        get {
            return audioMixer;
        }
    }

    public bool SoundMaster {
        get {
            return soundMaster;
        }

        set {
            soundMaster = value;
        }
    }

    public bool SoundEffects {
        get {
            return soundEffects;
        }

        set {
            soundEffects = value;
        }
    }

    public bool SoundMusic { get => soundMusic; set => soundMusic = value; }

    void Awake()
    {
        AddClickSource();

        AddStoreMusicAudioSource();
        AddClickAlternativeSource();
        AddMusicGameAudioSource();
        AddGameMenuMusicAudioSource();
        AddNeutralizerRayEffectAudioSource();
        AddExpansiveWaveEffectAudioSource();
        AddSelectionShipMusicGameAudioSource();
        AddSoundEnemyShipEngine();
        AddGarbageExplodingAudioSource();
        AddNeutralizerHitEffect();
        AddIntroMusicGameAudioSource();
        AddSelectionLevelMusicGameAudioSource();
        AddShipPlayerMovingAudioSource();
        AddMusicDeadAudioSource();
        AddMusicEnemyLaserSound();
        AddMusicWinAudioSource();
        AddMusicPlayerHit();
    }


    private void AddStoreMusicAudioSource()
    {
        audioSourceStoreMusic = gameObject.AddComponent<AudioSource>();
        audioSourceStoreMusic.clip = shopMusic;
        audioSourceStoreMusic.loop = true;
        audioSourceStoreMusic.outputAudioMixerGroup = audioMixer.FindMatchingGroups("Music")[0];
    }

    public void UpdateSounds()
    {
        audioMixer.SetFloatAsLinear("MusicVolumeValue", soundMusic ? volumeMausicStart : 0.0f);
        audioMixer.SetFloatAsLinear("SoundEffects", soundEffects ? volumeEffectsStart : 0.0f);

    }

    void Start()
    {

        soundMaster = true;
        soundMusic = true;
        soundEffects = true;
        audioMixer.GetFloatAsLinear("MusicVolumeValue", out volumeMausicStart);
        audioMixer.GetFloatAsLinear("SoundEffects", out volumeEffectsStart);

    }

    #region commonSounds
    private void AddClickSource()
    {
        audioSourceClick = gameObject.AddComponent<AudioSource>();
        audioSourceClick.clip = click;
        audioSourceClick.outputAudioMixerGroup = audioMixer.FindMatchingGroups("Sound Effects")[0];
    }

    internal void AddPlayerShipExplodingAudioSource(ref AudioSource audioSourcePlayerShipExploding)
    {
        audioSourcePlayerShipExploding = gameObject.AddComponent<AudioSource>();
        audioSourcePlayerShipExploding.outputAudioMixerGroup = audioMixer.FindMatchingGroups("Sound Effects")[0];
    }

    private void AddClickAlternativeSource()
    {
        audioSourceClickAlternative = gameObject.AddComponent<AudioSource>();
        audioSourceClickAlternative.clip = clickAlternative;
        audioSourceClickAlternative.outputAudioMixerGroup = audioMixer.FindMatchingGroups("Sound Effects")[0];
    }

    internal void AddPlayerShipMoving(ref AudioSource audioSourceShipMoving)
    {
        audioSourceShipMoving = gameObject.AddComponent<AudioSource>();
        audioSourceShipMoving.outputAudioMixerGroup = audioMixer.FindMatchingGroups("Sound Effects")[0];
    }

    internal void PlayPlayerShipExploding(AudioSource audioSourcePlayerShipExploding)
    {
        audioSourcePlayerShipExploding.Play();
    }
    internal void PlayMusicClick()
    {
        audioSourceClick.Play(0);
    }

    internal void PlayClickAlternative()
    {
        audioSourceClickAlternative.Play();
    }

    #endregion

    #region InGame
    private void AddMusicEnemyLaserSound()
    {
        audioSourceEnemyShootEffect = gameObject.AddComponent<AudioSource>();
        audioSourceEnemyShootEffect.clip = naveEnemyShotSound;
        audioSourceEnemyShootEffect.outputAudioMixerGroup = audioMixer.FindMatchingGroups("Sound Effects")[0];
    }

    private void AddSoundEnemyShipEngine()
    {
        audioSourceEnemyShootEffect = gameObject.AddComponent<AudioSource>();
        audioSourceEnemyShootEffect.clip = naveEnemyShotSound;
        audioSourceEnemyShootEffect.outputAudioMixerGroup = audioMixer.FindMatchingGroups("Sound Effects")[0];
    }

    private void AddMusicPlayerHit()
    {
        audioSourcePlayerHit = gameObject.AddComponent<AudioSource>();
        audioSourcePlayerHit.clip = playerHit;
        audioSourcePlayerHit.outputAudioMixerGroup = audioMixer.FindMatchingGroups("Sound Effects")[0];
    }

    private void AddNeutralizerHitEffect()
    {
        neutralizerSoundWhenRayHitShip = gameObject.AddComponent<AudioSource>();
        neutralizerSoundWhenRayHitShip.clip = neutralizerSoundHitEnemyShip;
        neutralizerSoundWhenRayHitShip.outputAudioMixerGroup = audioMixer.FindMatchingGroups("Sound Effects")[0];
    }


    private void AddMusicDeadAudioSource()
    {
        audioSourceLoseMusic = gameObject.AddComponent<AudioSource>();
        audioSourceLoseMusic.clip = loseMusic;
        audioSourceLoseMusic.loop = true;
        audioSourceLoseMusic.outputAudioMixerGroup = audioMixer.FindMatchingGroups("Music")[0];
    }

    public void AddEnemyShipExplodingAudioSource(ref AudioSource audioSourceEnemyShipExploding)
    {
        audioSourceEnemyShipExploding = gameObject.AddComponent<AudioSource>();
        audioSourceEnemyShipExploding.outputAudioMixerGroup = audioMixer.FindMatchingGroups("Sound Effects")[0];
    }

    public void AddGarbageExplodingAudioSource()
    {
        audioSourceGarbageDefeated = gameObject.AddComponent<AudioSource>();
        audioSourceGarbageDefeated.clip = garbageDestroyed;


        audioSourceGarbageDefeated.outputAudioMixerGroup = audioMixer.FindMatchingGroups("Sound Effects")[0];
    }

    private void AddMusicWinAudioSource()
    {
        audioSourceWinMusic = gameObject.AddComponent<AudioSource>();
        audioSourceWinMusic.clip = winMusic;
        audioSourceWinMusic.outputAudioMixerGroup = audioMixer.FindMatchingGroups("Sound Effects")[0];
    }

    private void AddNeutralizerRayEffectAudioSource()
    {
        audioSourceNeutralizerRayEffect = gameObject.AddComponent<AudioSource>();
        audioSourceNeutralizerRayEffect.clip = neutralizerRayShoot;
        audioSourceNeutralizerRayEffect.outputAudioMixerGroup = audioMixer.FindMatchingGroups("Sound Effects")[0];
    }

    private void AddExpansiveWaveEffectAudioSource()
    {
        audioSourceExpanaiveWaveEffect = gameObject.AddComponent<AudioSource>();
        audioSourceExpanaiveWaveEffect.clip = expansiveWave;
        audioSourceExpanaiveWaveEffect.outputAudioMixerGroup = audioMixer.FindMatchingGroups("Sound Effects")[0];
    }

    private void AddShipPlayerMovingAudioSource()
    {
        audioSourceShipPlayerMoving = gameObject.AddComponent<AudioSource>();
        audioSourceShipPlayerMoving.clip = shipMoving;
        audioSourceShipPlayerMoving.outputAudioMixerGroup = audioMixer.FindMatchingGroups("Sound Effects")[0];
        audioSourceShipPlayerMoving.loop = true;
    }

    internal void PlayNeutralizerHitAtEnemyShip()
    {
        neutralizerSoundWhenRayHitShip.Play();
    }

    internal void PlayGarbageDefeated()
    {
        audioSourceGarbageDefeated.Play();
    }
    internal void PlayEffectPlayerHit()
    {
        audioSourcePlayerHit.Play();
    }

    internal void PlayMusicStore()
    {
        audioSourceStoreMusic.Play();
    }
    internal void PlayEnemyShipExploding(AudioSource audioSourceEnemyShipExploding)
    {
        audioSourceEnemyShipExploding.Play();
    }
    internal void PlayExpansiveWaveEffect()
    {
        audioSourceExpanaiveWaveEffect.Play(0);
    }

    internal void PlayNeutralizerRayEffect()
    {
        audioSourceNeutralizerRayEffect.Play(0);
    }

    internal void StopSounds()
    {
        StopMusicDead();
        //StopShipMovingEffect();
        StopMusicWin();
    }

    internal void PlayShipMovingEffect(AudioSource audioSourceShipMoving)
    {
        audioSourceShipMoving.Play(0);
    }

    internal void PlayEnemyShootEffect()
    {
        audioSourceEnemyShootEffect.Play(0);
    }

    internal void PlayMusicDead()
    {
        audioSourceLoseMusic.Play(0);
    }
    internal void PlayMusicWin()
    {
        audioSourceWinMusic.Play(0);
    }

    internal void StopShipMovingEffect(AudioSource audioSourceShipMoving)
    {
        audioSourceShipMoving.Stop();
    }

    internal void StopMusicDead()
    {
        audioSourceLoseMusic.Stop();
    }

    internal void StopMusicWin()
    {
        audioSourceWinMusic.Stop();
    }

    internal void PlayStopMusicStore()
    {
        audioSourceStoreMusic.Stop();
    }


    #endregion

    #region Background music levels

    #endregion


    #region Menu selection Music
    private void AddSelectionLevelMusicGameAudioSource()
    {
        audioSourceSelectionLevelMusicGame = gameObject.AddComponent<AudioSource>();
        audioSourceSelectionLevelMusicGame.clip = selectionLevelMusic;
        audioSourceSelectionLevelMusicGame.loop = true;
        audioSourceSelectionLevelMusicGame.outputAudioMixerGroup = audioMixer.FindMatchingGroups("Music")[0];
    }

    private void AddSelectionShipMusicGameAudioSource()
    {
        audioSourceSelectionShipMusicGame = gameObject.AddComponent<AudioSource>();
        audioSourceSelectionShipMusicGame.clip = selectionShipMusic;
        audioSourceSelectionShipMusicGame.loop = true;
        audioSourceSelectionShipMusicGame.outputAudioMixerGroup = audioMixer.FindMatchingGroups("Music")[0];
    }
    private void AddMusicGameAudioSource()
    {
        audioSourceintroMusicGame = gameObject.AddComponent<AudioSource>();
        audioSourceintroMusicGame.clip = musicGame;
        audioSourceintroMusicGame.loop = true;
        audioSourceintroMusicGame.outputAudioMixerGroup = audioMixer.FindMatchingGroups("Music")[0];
    }


    private void AddGameMenuMusicAudioSource()
    {
        audioSourceGameMenuMusic = gameObject.AddComponent<AudioSource>();
        audioSourceGameMenuMusic.clip = gameMenuMusic;
        audioSourceGameMenuMusic.loop = true;
        audioSourceGameMenuMusic.outputAudioMixerGroup = audioMixer.FindMatchingGroups("Music")[0];
    }
    private void AddIntroMusicGameAudioSource()
    {
        audioSourceintroMusicGame = gameObject.AddComponent<AudioSource>();
        audioSourceintroMusicGame.clip = introMusicGame;
        audioSourceintroMusicGame.loop = true;
        audioSourceintroMusicGame.outputAudioMixerGroup = audioMixer.FindMatchingGroups("Music")[0];
    }




    internal void PlaySelectionlevelMusicGame()
    {
        audioSourceSelectionLevelMusicGame.Play(0);
    }


    internal void PlayGameMenuMusic()
    {
        audioSourceGameMenuMusic.Play(0);
    }
    internal void PlaySelectionShipMusicGame()
    {
        audioSourceSelectionShipMusicGame.Play(0);
    }

    internal void StopSelectionShipMusicGame()
    {
        audioSourceSelectionShipMusicGame.Stop();
    }


    internal void StopGameMenuMusic()
    {
        audioSourceGameMenuMusic.Stop();
    }
    internal void PlayIntroMusicGame()
    {
        this.audioSourceintroMusicGame.Play(0);
    }

    internal void StopGameMusic()
    {
        audioSourceintroMusicGame.Stop();
    }

    internal void StopIntroMusic()
    {
        audioSourceintroMusicGame.Stop();
    }

    internal void StopSelectionlevelMusic()
    {
        audioSourceSelectionLevelMusicGame.Stop();
    }

    #endregion











    //Todo 
    //Añadir sonido cuando empezar nivel al pulsar el boton Play de la ventana Loading

}

