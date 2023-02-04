using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MusicPlayer : MonoBehaviour {
    [System.Serializable]
    public class Music {
        public string name;
        public string author;
        public AudioClip song;

        public override string ToString() {
            return this.name + " by " + this.author;
        }
    }

    #region Class Instance
    public static MusicPlayer instance = null;
    private void CreateInstance() {
        if (instance == null)
            instance = this;
        else
            Destroy(this);
    }
    #endregion
    private void Awake() => CreateInstance();

    [SerializeField] private AudioSource audioSource;
    [SerializeField] private WindowManager window;
    [SerializeField] private GameObject desktopPlayer;
    [SerializeField] private Image playPauseImage;
    [SerializeField] private Sprite playIcon;
    [SerializeField] private Sprite pauseIcon;
    [SerializeField] private Music[] songs;
    [SerializeField] private MusicTitleWrapper[] titleWrappers;

    private Dictionary<string, Music> playlist;
    private List<string> titles;
    private string currentSong;
    private int index = 0;

    public bool IsPaused { get; private set; } = true;

    private void Start() {
        this.playlist = new Dictionary<string, Music>();
        this.titles = new List<string>();
        foreach (Music music in this.songs) {
            if (this.playlist.TryAdd(music.name, music)) {
                this.titles.Add(music.name);
            }
        }
        this.titles.Sort();
        this.currentSong = this.titles[this.index];
        this.audioSource.clip = this.playlist[this.currentSong].song;
        foreach (string song in this.titles) {
            this.window.Add(song, () => {
                PlaySong(song);
            });
        }
    }

    private void FixedUpdate() {
        if (!this.audioSource.isPlaying && !this.IsPaused) {
            this.NextSong();
        }
        if (this.audioSource.isPlaying) {
            if (this.window.IsOpen) {
                this.desktopPlayer.SetActive(false);
            } else {
                this.desktopPlayer.SetActive(true);
                UpdateTitles();
            }
        }
    }

    public string GetCurrentMusic() {
        if (this.IsPaused) return "Music Paused";
        return this.playlist[currentSong].ToString();
    }

    public void Play() {
        if (this.IsPaused) {
            this.IsPaused = false;
            if (this.audioSource.time > 0) {
                this.audioSource.UnPause();
            } else {
                this.audioSource.Play();
            }
            if (this.window.IsOpen) {
                this.playPauseImage.sprite = this.pauseIcon;
            }
        } else {
            this.IsPaused = true;
            this.audioSource.Pause();
            if (this.window.IsOpen) {
                this.playPauseImage.sprite = this.playIcon;
            }
        }
        UpdateTitles();
    }

    public void NextSong() {
        this.audioSource.Stop();
        this.index++;
        if (this.index >= this.titles.Count) {
            this.index = 0;
        }
        ChangeSong();
    }

    public void PreviousSong() {
        this.audioSource.Stop();
        this.index--;
        if (this.index < 0) {
            this.index = this.titles.Count - 1;
        }
        ChangeSong();
    }

    public void PlaySong(string song) {
        this.audioSource.Stop();
        this.currentSong = song;
        this.index = FindIndex(song);
        this.audioSource.clip = this.playlist[this.currentSong].song;
        this.audioSource.Play();
        this.IsPaused = false;
        if (this.window.IsOpen) {
            this.playPauseImage.sprite = this.pauseIcon;
        }
        UpdateTitles();
    }

    public void ChangeVolumeLevel(float level) {
        this.audioSource.volume = level;
    }

    public float GetVolumeLevel() {
        return this.audioSource.volume;
    }

    public float GetSongLength() {
        return this.audioSource.clip.length;
    }

    public float GetSongTime() {
        return this.audioSource.time;
    }

    private void ChangeSong(string song = null) {
        if (song == null) {
            this.currentSong = this.titles[this.index];
        } else {
            this.currentSong = song;
        }
        UpdateTitles();
        this.audioSource.clip = this.playlist[this.currentSong].song;
        this.audioSource.Play();
        this.IsPaused = false;
        if (this.window.IsOpen) {
            this.playPauseImage.sprite = this.pauseIcon;
        }
        UpdateTitles();
    }

    private int FindIndex(string name) {
        for (int i = 0; i < this.titles.Count; i++) {
            if (this.titles[i] == name) return i;
        }
        return -1;
    }

    private void UpdateTitles() {
        foreach (MusicTitleWrapper wrapper in this.titleWrappers) {
            if (wrapper.gameObject.activeSelf) {
                wrapper.UpdateTitle(GetCurrentMusic());
            }
        }
    }
}
