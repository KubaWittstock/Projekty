import React, { useState, useEffect } from "react";

import List from "@mui/material/List";
import ListItem from "@mui/material/ListItem";
import ListItemButton from "@mui/material/ListItemButton";
import ListItemText from "@mui/material/ListItemText";
import ListItemAvatar from "@mui/material/ListItemAvatar";
import Checkbox from "@mui/material/Checkbox";
import Avatar from "@mui/material/Avatar";
import { Grid } from "@material-ui/core";

export default function ItemList(props) {
  const [checked, setChecked] = useState([]);
  const maxChecked = checked.filter((v) => v).length >= 5;
  const handleToggle = (value) => () => {
    const currentIndex = checked.indexOf(value);
    const newChecked = [...checked];
    if (currentIndex === -1) {
      if (!maxChecked) {
        newChecked.push(value);
      }
    } else {
      newChecked.splice(currentIndex, 1);
    }
    setChecked(newChecked);
  };
  const countChecked = checked.length;
  props.passChildData(countChecked);
  return (
    <Grid item xs={4} align="center">
      <List
        dense
        sx={{
          width: "100%",
          maxWidth: 360,
          maxHeight: 600,
          overflow: "auto",
          bgcolor: "background.paper",
        }}
      >
        {props.data.map((value) => {
          const labelId = `checkbox-list-secondary-label-${value}`;
          return (
            <ListItem
              key={value}
              secondaryAction={
                <Checkbox
                  edge="end"
                  onChange={handleToggle(value)}
                  checked={checked.indexOf(value) !== -1}
                  inputProps={{ "aria-labelledby": labelId }}
                  disabled={maxChecked && !(checked.indexOf(value) !== -1)}
                />
              }
              disablePadding
            >
              <ListItemButton>
                <ListItemAvatar>
                  <Avatar alt={`Avatar n°${value + 1}`} src={value.img} />
                </ListItemAvatar>
                <ListItemText id={labelId} primary={value.name} />
              </ListItemButton>
            </ListItem>
          );
        })}
      </List>
    </Grid>
  );
}
