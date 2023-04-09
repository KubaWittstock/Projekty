import * as React from "react";
import Backdrop from "@mui/material/Backdrop";
import CircularProgress from "@mui/material/CircularProgress";
import Box from "@mui/material/Box";
import Typography from "@mui/material/Typography";

export default function SimpleBackdrop({ open }) {
  return (
    <div>
      <Backdrop
        sx={{ color: "#fff", zIndex: (theme) => theme.zIndex.drawer + 1 }}
        open={open}
      >
        <Box>
          <CircularProgress color="inherit" />
          <Typography component="h6" variant="h6">
            Be right back..
          </Typography>
          <Typography component="h6" variant="h6">
            Brb, making your perfect playlist...
          </Typography>
        </Box>
      </Backdrop>
    </div>
  );
}
