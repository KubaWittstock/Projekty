import React, { useState, useEffect } from "react";
import { useNavigate } from "react-router-dom";
import { Grid, Button, Typography } from "@material-ui/core";

import CircularProgress from "@mui/material/CircularProgress";
import Box from "@mui/material/Box";
import DiscreteSlider from "./Slider";
import MySwitch from "./Switch";
import TopItemsList from "./List";
import SimpleBackdrop from "./Backdrop";
import ActionAlerts from "./Alert";
import FormControl from "@mui/material/FormControl";
import Input from "@mui/material/Input";
import InputLabel from "@mui/material/InputLabel";

function PlaylistGenerator() {
  const navigate = useNavigate();
  const maxChecked = 15;
  const [loadingOpen, setLoadingOpen] = useState(false);
  const [loading, setLoading] = useState(true);
  const [showAlert, setShowAlert] = useState(false);
  const [alertProps, setAlertProps] = useState({
    text: "Success",
    severity: "success",
  });
  const [userName, setUserName] = useState(null);
  const [playlistID, setPlaylistID] = useState("");
  const [userTopArtists, setUserTopArtists] = useState(null);
  const [userTopTracks, setUserTopTracks] = useState(null);
  const [userTopGenres, setUserTopGenres] = useState(null);
  const [checkedArtists, setCheckedArtists] = useState({
    name: Array(),
    uri: "",
  });
  const [checkedTracks, setCheckedTracks] = useState({
    name: Array(),
    uri: "",
  });
  const [checkedGenres, setCheckedGenres] = useState({
    name: Array(),
    uri: "",
  });
  const selectedItems =
    checkedArtists.name.length +
    checkedTracks.name.length +
    checkedGenres.name.length;
  const [noTracks, setNoTracks] = useState(20);
  const [additionalSettings, setAdditionalSettings] = useState(false);
  const [playlistEnergy, setPlaylistEnergy] = useState(50);
  const [playlistValence, setPlaylistValence] = useState(50);

  function CircularIndeterminate() {
    return (
      <Grid container spacing={2} className="center2">
        <Grid item xs={12} align="center">
          <Box>
            <CircularProgress />
          </Box>
          <Typography component="h6" variant="h6">
            Loading..
          </Typography>
        </Grid>
      </Grid>
    );
  }

  useEffect(
    async () =>
      fetch("/spotify_backend/isAuthenticated")
        .then((response) => response.json())
        .then((data) => {
          if (!data.status) {
            navigate("/");
          }
        }),
    []
  );

  useEffect(() => {
    const fetchData = async () => {
      const uName = await fetch("spotify_backend/getUserProfile").then(
        (response) => response.json()
      );
      const uTopItems = await fetch("spotify_backend/getUserTopItems").then(
        (response) => response.json()
      );

      setUserName(uName.user_name);
      setUserTopArtists(uTopItems.artists);
      setUserTopTracks(uTopItems.tracks);
      setUserTopGenres(uTopItems.genres);
      setLoading(false);
    };
    fetchData();
  }, []);

  function CreatePlaylist() {
    setShowAlert(false);
    if (selectedItems > 0) {
      setLoadingOpen(true);
      fetch(
        `spotify_backend/createPlaylist/${checkedArtists.uri}&${checkedTracks.uri}&${checkedGenres.uri}&${noTracks}&${additionalSettings}&${playlistValence}&${playlistEnergy}`
      )
        .then((response) => response.json())
        .then((data) => {
          setLoadingOpen(false);
          return data;
        })
        .then((data) => {
          if (data.error) {
            setAlertProps({ text: data.error, severity: "error" });
            setShowAlert(true);
          } else {
            setAlertProps({ text: "Playlist created!", severity: "success" });
            setShowAlert(true);
          }
        });
    } else {
      setAlertProps({
        text: "You must pick at least 1 item!",
        severity: "error",
      });
      setShowAlert(true);
    }
  }

  function GetPlaylist() {
    setShowAlert(false);
    if (playlistID.length > 0) {
      setLoadingOpen(true);
      const link = playlistID.split("/");
      const query = link.length > 0 ? link.findLast((x) => x) : "";
      const addSet = additionalSettings ? 1 : 0;
      fetch(
        `spotify_backend/getTracksFromPlaylist/${noTracks}&${addSet}&${playlistValence}&${playlistEnergy}&${query}`
      )
        .then((response) => response.json())
        .then((data) => {
          setLoadingOpen(false);
          return data;
        })
        .then((data) => {
          if (data.error) {
            setAlertProps({ text: data.error, severity: "error" });
            setShowAlert(true);
          } else {
            setAlertProps({ text: "Playlist created!", severity: "success" });
            setShowAlert(true);
          }
        });
    } else {
      setAlertProps({
        text: "You must provide valid URL to spotify playlist!",
        severity: "error",
      });
      setShowAlert(true);
    }
  }

  useEffect(
    async () =>
      fetch("/spotify_backend/isAuthenticated")
        .then((response) => response.json())
        .then((data) => {
          if (!data.status) {
            navigate("/");
          }
        }),
    []
  );
  const handleChange = (e) => {
    setPlaylistID(e.target.value);
  };

  if (loading) return CircularIndeterminate();
  return (
    <Grid container spacing={2}>
      <Grid item xs={12} align="center">
        <Typography component="h4" variant="h4">
          Hello {userName},
        </Typography>
        <Typography component="h4" variant="h4">
          here are your top 50 items on Spotify:
        </Typography>
      </Grid>
      <Grid item xs={12} align="center">
        <Typography component="h6" variant="h6">
          Chose up to 15 items (max 5 per category) to create your Perfect
          Playlist...
        </Typography>
      </Grid>
      <Grid container spacing={0} align="center" justifyContent="center">
        <TopItemsList
          items={userTopArtists}
          maxChecked={5}
          onChange={setCheckedArtists}
          name="Artists"
        />
        <TopItemsList
          items={userTopTracks}
          maxChecked={5}
          onChange={setCheckedTracks}
          name="Tracks"
        />
        <TopItemsList
          items={userTopGenres}
          maxChecked={5}
          onChange={setCheckedGenres}
          name="Genres"
        />
      </Grid>
      <Grid item xs={12} align="center">
        <Box
          sx={{
            p: 1,
            width: 1150,
            maxWidth: "85%",
          }}
        >
          <Typography>...or just paste link to playlist here.</Typography>
          <FormControl
            variant="standard"
            sx={{
              width: 1150,
              maxWidth: "85%",
            }}
          >
            <InputLabel htmlFor="component-simple">Playlist URL</InputLabel>
            <Input id="component-simple" onBlur={handleChange} />
          </FormControl>
        </Box>
      </Grid>
      <Grid item xs={12} align="center">
        <DiscreteSlider
          onChange={setNoTracks}
          defaultValue={noTracks}
          text={"Taget no. tracks"}
          marks={[
            { value: 10, label: "10" },
            { value: 100, label: "100" },
          ]}
        />
      </Grid>
      <Grid item xs={12} align="center">
        <MySwitch
          defaultValue={additionalSettings}
          onChange={setAdditionalSettings}
          text="Additional Settings"
        />
      </Grid>
      <Grid item xs={12} align="center">
        <DiscreteSlider
          onChange={setPlaylistEnergy}
          defaultValue={playlistEnergy}
          text="Downbeat / Upbeat"
          marks={[
            { value: 0, label: "Downbeat" },
            { value: 100, label: "Upbeat" },
          ]}
          step={1}
          min={1}
          max={100}
          disabled={!additionalSettings}
        />
      </Grid>
      <Grid item xs={12} align="center">
        <DiscreteSlider
          onChange={setPlaylistValence}
          defaultValue={playlistValence}
          text="Mellow / Energetic"
          marks={[
            { value: 0, label: "Mellow" },
            { value: 100, label: "Energetic" },
          ]}
          step={1}
          min={1}
          max={100}
          disabled={!additionalSettings}
        />
      </Grid>
      <Grid item xs={12} align="center">
        <SimpleBackdrop open={loadingOpen} />
      </Grid>
      <Grid item xs={12} align="center">
        <ActionAlerts
          open={showAlert}
          callback={setShowAlert}
          severity={alertProps.severity}
          text={alertProps.text}
        />
      </Grid>
      <Grid item xs={12} align="center">
        <Box sx={{ p: 1 }}>
          <Button
            color="primary"
            variant="contained"
            onClick={() => {
              CreatePlaylist();
            }}
          >
            Create with settings
          </Button>
        </Box>
        <Box>
          <Button
            color="secondary"
            variant="contained"
            onClick={() => {
              GetPlaylist();
            }}
          >
            Create with playlist
          </Button>
        </Box>
      </Grid>
    </Grid>
  );
}

export default PlaylistGenerator;
