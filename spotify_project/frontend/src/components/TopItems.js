import * as React from "react";
import TopItemsList from "./List";
import Grid from "@mui/material/Grid";
import Typography from "@mui/material/Typography";

export default function TopItems(props) {
  console.log(props.topCategories);
  return (
    <Grid container spacing={2}>
      <Grid item xs={12} align="center">
        {props.topCategories.map((value) => {
          console.log(value);
          <TopItemsList items={value} maxChecked={5} />;
        })}
      </Grid>
    </Grid>
  );
}
