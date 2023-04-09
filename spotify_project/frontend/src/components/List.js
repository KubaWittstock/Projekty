import * as React from "react";
import Box from "@mui/material/Box";
import List from "@mui/material/List";
import ListItem from "@mui/material/ListItem";
import ListItemButton from "@mui/material/ListItemButton";
import ListItemAvatar from "@mui/material/ListItemAvatar";
import ListItemText from "@mui/material/ListItemText";
import Checkbox from "@mui/material/Checkbox";
import Avatar from "@mui/material/Avatar";
import Typography from "@mui/material/Typography";

export default function TopItemsList(props) {
  const [checked, setChecked] = React.useState([]);
  const item = props.name || "";
  const maxChecked = props.maxChecked;
  const countChecked = checked.length;
  const isMax = countChecked >= maxChecked;

  const handleToggle = (value) => () => {
    const currentIndex = checked.indexOf(value);
    const newChecked = [...checked];

    if (currentIndex === -1) {
      if (!isMax) {
        newChecked.push(value);
      }
    } else {
      newChecked.splice(currentIndex, 1);
    }
    const checkedURI = newChecked
      .map((value) => value.uri.split(":").slice(-1))
      .join(",");
    const name = newChecked.map((value) => value.name);
    setChecked(newChecked);
    props.onChange({ name: name, uri: checkedURI });
  };
  const isImg = Object.hasOwn(props.items[0], "img");
  const items = props.items;

  const listItems = items.map((value) => {
    const labelId = `checkbox-list-secondary-label-${value}`;
    return (
      <ListItem key={value} disablePadding>
        <ListItemButton
          onClick={handleToggle(value)}
          disabled={isMax && !(checked.indexOf(value) !== -1)}
          sx={{
            minWidth: 100,
          }}
          dense
        >
          {isImg && (
            <ListItemAvatar edge="start">
              <Avatar alt={`Avatar n°${value + 1}`} src={value.img} />
            </ListItemAvatar>
          )}
          <ListItemText id={labelId} primary={value.name} />
          <Checkbox
            edge="end"
            checked={checked.indexOf(value) !== -1}
            tabIndex={-1}
            disableRipple
            inputProps={{ "aria-labelledby": labelId }}
          />
        </ListItemButton>
      </ListItem>
    );
  });

  return (
    <Box sx={{ width: "100%", maxWidth: 360, display: "inline" }}>
      <Typography component="h6" variant="h6">
        {item}
      </Typography>
      <Typography component="h6" variant="h6">
        {countChecked} / {maxChecked}
      </Typography>
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
        {listItems}
      </List>
    </Box>
  );
}
