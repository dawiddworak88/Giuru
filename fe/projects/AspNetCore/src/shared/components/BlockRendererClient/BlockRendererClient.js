import React from "react";
import {
  BlocksRenderer,
} from "@strapi/blocks-react-renderer";

const BlockRendererClient = ({
  content,
}) => {
  if (!content) return null;
  
  return (
    <BlocksRenderer
      content={content}
    />
  );
}

export default BlockRendererClient;