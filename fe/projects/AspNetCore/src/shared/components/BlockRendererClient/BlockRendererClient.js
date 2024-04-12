import React from "react";
import {
  BlocksRenderer,
} from "@strapi/blocks-react-renderer";

const BlockRendererClient = ({
  content,
}) => {
  console.log(content);
  
  if (!content) return null;
  
  return (
    content.length > 0 && (
      <BlocksRenderer
        content={content}
      />
    )
  );
}

export default BlockRendererClient;