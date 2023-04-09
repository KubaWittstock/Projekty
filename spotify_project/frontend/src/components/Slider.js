import React, { useState, useEffect } from "react";
import Box from "@mui/material/Box";
import Slider from "@mui/material/Slider";
import Typography from "@mui/material/Typography";
import Stack from "@mui/material/Stack";

export default function DiscreteSlider(props) {
  const [value, setValue] = useState(props.defaultValue);
  const text = props.text || null;
  const min = props.min || 10;
  const max = props.max || 100;
  const step = props.step || 10;
  const marks = props.marks || null;
  const disabled = props.disabled || false;

  const innerHandleChange = (event, newValue) => {
    if (typeof newValue === "number") {
      if (newValue !== value) {
        setValue(newValue);
      }
    }
  };
  const outerHandleChange = (event, newValue) => {
    props.onChange(value);
  };

  function renderLabel() {
    return text ? <Typography>{text}</Typography> : null;
  }

  return (
    <Box sx={{ width: "70%", maxWidth: 1000 }}>
      {renderLabel()}
      <Stack spacing={2} direction="row" sx={{ mb: 1 }} alignItems="center">
        <Slider
          aria-label="No. tracks"
          defaultValue={value}
          disabled={disabled}
          aria-labelledby="track-false-slider"
          step={step}
          min={min}
          max={max}
          marks={marks}
          onChange={innerHandleChange}
          onChangeCommitted={outerHandleChange}
          valueLabelDisplay="auto"
        />
      </Stack>
    </Box>
  );
}
