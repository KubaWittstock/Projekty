import React, { useState, useEffect } from "react";
import Box from "@mui/material/Box";
import Switch from "@mui/material/Switch";
import Typography from "@mui/material/Typography";
import Stack from "@mui/material/Stack";

export default function MySwitch(props) {
  const [checked, setChecked] = useState(props.defaultValue);
  const text = props.text || "aaa";

  const handleChange = (event) => {
    setChecked(event.target.checked);
    props.onChange(event.target.checked);
  };

  function renderLabel() {
    return text ? <Typography>{text}</Typography> : null;
  }

  return (
    <Box sx={{ width: "70%", maxWidth: 1000 }}>
      <Stack spacing={2} direction="row" sx={{ mb: 1 }} alignItems="center">
        <Switch
          checked={checked}
          onChange={handleChange}
          inputProps={{ "aria-label": "controlled" }}
        />
        {renderLabel()}
      </Stack>
    </Box>
  );
}
