import React from "react";
import PropTypes from "prop-types";

function TextWidget(props) {
  return <div>Dawid</div>;
}

TextWidget.propTypes = {
  value: PropTypes.oneOfType([PropTypes.string, PropTypes.number]),
  id: PropTypes.string,
};

export default TextWidget;