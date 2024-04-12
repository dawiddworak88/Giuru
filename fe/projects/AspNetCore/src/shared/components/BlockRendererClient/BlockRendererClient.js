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
      content={JSON.parse(content) | []}
    />
  );
}

export default BlockRendererClient;