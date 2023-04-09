import PlaylistGenerator from "./PlaylistGenerator";
import React, { useState, useEffect } from "react";
import { Grid, Button, ButtonGroup, Typography } from "@material-ui/core";
import { Image } from "mui-image";
import {
  BrowserRouter as Router,
  Routes,
  Route,
  Link,
  Redirect,
  Navigate,
} from "react-router-dom";

function HomePage() {
  const [isAuth, setIsAuth] = useState(false);

  useEffect(
    async () =>
      fetch("/spotify_backend/isAuthenticated")
        .then((response) => response.json())
        .then((data) => {
          setIsAuth(data.status);
        }),
    []
  );

  return (
    <Router>
      <Routes>
        <Route exaxt path="/" element={RenderOrRedirect()}></Route>
        <Route
          path="/playlistGenerator"
          element={<PlaylistGenerator />}
        ></Route>
      </Routes>
    </Router>
  );

  function RenderHomePage() {
    return (
      <Grid container spacing={3} className="center3">
        <Grid item xs={12} align="center">
          <img src="/static/images/spotify2.png" width={"300px"}></img>
        </Grid>
        <Grid item xs={12} align="center">
          <ButtonGroup disableElevation variant="contained" color="primary">
            <Button color="#f0f0f0" onClick={AuthenticateUser}>
              Connect with Spotify
            </Button>
          </ButtonGroup>
        </Grid>
      </Grid>
    );
  }

  function AuthenticateUser() {
    if (!isAuth) {
      fetch("/spotify_backend/getAuthenticationURL")
        .then((response) => response.json())
        .then((data) => {
          window.location.replace(data.url);
        });
    }
  }
  function RenderOrRedirect() {
    return isAuth ? (
      <Navigate replace to={`/playlistGenerator`} />
    ) : (
      RenderHomePage()
    );
  }
}

export default HomePage;
