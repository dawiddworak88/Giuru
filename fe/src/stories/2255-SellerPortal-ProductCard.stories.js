import React from "react";
import ProductCard from "../project/Seller.Portal/areas/Products/pages/ProductCardPage/ProductCardPage";
import { header, menuTiles, footer } from "./Shared/Props";
import "../project/Seller.Portal/areas/Products/pages/ProductCardPage/ProductCardPage.scss";

const componentProps = {
  title: "Edit product card",
  navigateToProductCardsLabel: "Back to products cards",
  uiSchema: null,
  fieldRequiredErrorMessage: "Field is required",
  generalErrorMessage: "An Error Occurred",
  saveUrl: "",
  saveText: "Save",
  defaultInputName: "NewElement",
  newText: "Add new card",
  schema: '{}'
  // schema: `{
  //   "definitions": {
  //     "a04b3368-fa25-4b4a-e4eb-08d907680a85": {
  //       "title": "Color",
  //       "type": "string",
  //       "anyOf": [
  //         {
  //           "type": "string",
  //           "enum": [
  //             "531f3581-510b-48a0-5ce5-08d907680c53"
  //           ],
  //           "title": "Beech Wood"
  //         },
  //         {
  //           "type": "string",
  //           "enum": [
  //             "71399480-3539-47a0-5ce6-08d907680c53"
  //           ],
  //           "title": "Beige"
  //         },
  //         {
  //           "type": "string",
  //           "enum": [
  //             "f94d46da-f7b5-44f2-5ce7-08d907680c53"
  //           ],
  //           "title": "Black"
  //         },
  //         {
  //           "type": "string",
  //           "enum": [
  //             "98cfb696-5680-4bbd-5ce8-08d907680c53"
  //           ],
  //           "title": "Blue"
  //         },
  //         {
  //           "type": "string",
  //           "enum": [
  //             "5318b422-a5d6-4520-5ce9-08d907680c53"
  //           ],
  //           "title": "Brown"
  //         },
  //         {
  //           "type": "string",
  //           "enum": [
  //             "ac863443-8858-4929-5cea-08d907680c53"
  //           ],
  //           "title": "Brown Wood"
  //         },
  //         {
  //           "type": "string",
  //           "enum": [
  //             "6e00d7f4-cf3d-4657-5ceb-08d907680c53"
  //           ],
  //           "title": "Colorful"
  //         },
  //         {
  //           "type": "string",
  //           "enum": [
  //             "31e3f58f-80fd-47ed-5cec-08d907680c53"
  //           ],
  //           "title": "Cream"
  //         },
  //         {
  //           "type": "string",
  //           "enum": [
  //             "ce5931f3-2cc8-41a5-5ced-08d907680c53"
  //           ],
  //           "title": "Dark Beige"
  //         },
  //         {
  //           "type": "string",
  //           "enum": [
  //             "7736646a-8e13-4817-5cee-08d907680c53"
  //           ],
  //           "title": "Dark Black"
  //         },
  //         {
  //           "type": "string",
  //           "enum": [
  //             "7979c6b2-7ce3-4cbd-5cef-08d907680c53"
  //           ],
  //           "title": "Dark Blue"
  //         },
  //         {
  //           "type": "string",
  //           "enum": [
  //             "4ab85c3a-29de-4b87-5cf0-08d907680c53"
  //           ],
  //           "title": "Dark Brown"
  //         },
  //         {
  //           "type": "string",
  //           "enum": [
  //             "beba6247-94f8-41aa-5cf1-08d907680c53"
  //           ],
  //           "title": "Dark Gray"
  //         },
  //         {
  //           "type": "string",
  //           "enum": [
  //             "83c9dcf0-2fe7-4c3d-5cf2-08d907680c53"
  //           ],
  //           "title": "Dark Green"
  //         },
  //         {
  //           "type": "string",
  //           "enum": [
  //             "d3087ad0-c789-42ba-5cf3-08d907680c53"
  //           ],
  //           "title": "Dark Purple"
  //         },
  //         {
  //           "type": "string",
  //           "enum": [
  //             "16a4b5c8-a48a-47eb-5cf4-08d907680c53"
  //           ],
  //           "title": "Flowers"
  //         },
  //         {
  //           "type": "string",
  //           "enum": [
  //             "cdbc2fcb-6a73-48a6-5cf5-08d907680c53"
  //           ],
  //           "title": "Gray"
  //         },
  //         {
  //           "type": "string",
  //           "enum": [
  //             "719bcc73-6e00-4d38-5cf6-08d907680c53"
  //           ],
  //           "title": "Green"
  //         },
  //         {
  //           "type": "string",
  //           "enum": [
  //             "bd2905ae-9ef4-4c47-5cf7-08d907680c53"
  //           ],
  //           "title": "Light Brown"
  //         },
  //         {
  //           "type": "string",
  //           "enum": [
  //             "e60c8cd3-31d4-4c97-5cf8-08d907680c53"
  //           ],
  //           "title": "Metal"
  //         },
  //         {
  //           "type": "string",
  //           "enum": [
  //             "31ddb271-cbbd-4c4f-5cf9-08d907680c53"
  //           ],
  //           "title": "Orange"
  //         },
  //         {
  //           "type": "string",
  //           "enum": [
  //             "b0ecfa80-643f-46f4-5cfa-08d907680c53"
  //           ],
  //           "title": "Pink"
  //         },
  //         {
  //           "type": "string",
  //           "enum": [
  //             "2ca4babf-b84d-4864-5cfb-08d907680c53"
  //           ],
  //           "title": "Purple"
  //         },
  //         {
  //           "type": "string",
  //           "enum": [
  //             "81fa4a95-aff6-480d-5cfc-08d907680c53"
  //           ],
  //           "title": "Red"
  //         },
  //         {
  //           "type": "string",
  //           "enum": [
  //             "6d174e56-d1a6-4f92-5cfd-08d907680c53"
  //           ],
  //           "title": "Rose"
  //         },
  //         {
  //           "type": "string",
  //           "enum": [
  //             "1a5e7e9b-f643-4d98-5cfe-08d907680c53"
  //           ],
  //           "title": "Silver"
  //         },
  //         {
  //           "type": "string",
  //           "enum": [
  //             "71ce3838-0b25-49ab-5cff-08d907680c53"
  //           ],
  //           "title": "Sonoma"
  //         },
  //         {
  //           "type": "string",
  //           "enum": [
  //             "1a771083-06a1-4b28-5d00-08d907680c53"
  //           ],
  //           "title": "Walnut Wood"
  //         },
  //         {
  //           "type": "string",
  //           "enum": [
  //             "ebce1860-a899-4b01-5d01-08d907680c53"
  //           ],
  //           "title": "White"
  //         },
  //         {
  //           "type": "string",
  //           "enum": [
  //             "1af46bc1-57f1-4a6a-5d02-08d907680c53"
  //           ],
  //           "title": "Wood"
  //         },
  //         {
  //           "type": "string",
  //           "enum": [
  //             "0d77c731-9ad3-42bf-5d03-08d907680c53"
  //           ],
  //           "title": "Wood Brown"
  //         },
  //         {
  //           "type": "string",
  //           "enum": [
  //             "67d963b6-3cda-4827-5d04-08d907680c53"
  //           ],
  //           "title": "Yellow"
  //         },
  //         {
  //           "type": "string",
  //           "enum": [
  //             "f66577ad-2d9d-4280-0f91-08d963d000b5"
  //           ],
  //           "title": "Glass"
  //         }
  //       ]
  //     },
  //     "be472892-6902-4736-e4ec-08d907680a85": {
  //       "title": "Fabrics",
  //       "type": "string",
  //       "anyOf": [
  //         {
  //           "type": "string",
  //           "enum": [
  //             "1ea16d52-4fc0-4e81-5d05-08d907680c53"
  //           ],
  //           "title": "Ac 01"
  //         },
  //         {
  //           "type": "string",
  //           "enum": [
  //             "0e1979fd-ade0-4a95-5d06-08d907680c53"
  //           ],
  //           "title": "Alova 04"
  //         },
  //         {
  //           "type": "string",
  //           "enum": [
  //             "77ea32f4-d284-4312-5d07-08d907680c53"
  //           ],
  //           "title": "Alova 07"
  //         },
  //         {
  //           "type": "string",
  //           "enum": [
  //             "e3f4f91a-723f-41d9-5d08-08d907680c53"
  //           ],
  //           "title": "Alova 10"
  //         },
  //         {
  //           "type": "string",
  //           "enum": [
  //             "ed9c36b7-dd44-448b-5d09-08d907680c53"
  //           ],
  //           "title": "Alova 12"
  //         },
  //         {
  //           "type": "string",
  //           "enum": [
  //             "a9e45f32-73fd-472a-5d0a-08d907680c53"
  //           ],
  //           "title": "Alova 29"
  //         },
  //         {
  //           "type": "string",
  //           "enum": [
  //             "4792951d-1887-421e-5d0b-08d907680c53"
  //           ],
  //           "title": "Alova 36"
  //         },
  //         {
  //           "type": "string",
  //           "enum": [
  //             "79bc72b0-7251-4d0e-5d0c-08d907680c53"
  //           ],
  //           "title": "Alova 42"
  //         },
  //         {
  //           "type": "string",
  //           "enum": [
  //             "35813384-4be9-4468-5d0d-08d907680c53"
  //           ],
  //           "title": "Alova 46"
  //         },
  //         {
  //           "type": "string",
  //           "enum": [
  //             "b051b114-cc49-46b3-5d0e-08d907680c53"
  //           ],
  //           "title": "Alova 48"
  //         },
  //         {
  //           "type": "string",
  //           "enum": [
  //             "c9f2e42d-f502-474c-5d0f-08d907680c53"
  //           ],
  //           "title": "Alova 66"
  //         },
  //         {
  //           "type": "string",
  //           "enum": [
  //             "4a1e81e3-1073-41d2-5d10-08d907680c53"
  //           ],
  //           "title": "Alova 67"
  //         },
  //         {
  //           "type": "string",
  //           "enum": [
  //             "7b67d0c0-8079-49cb-5d11-08d907680c53"
  //           ],
  //           "title": "Alova 68"
  //         },
  //         {
  //           "type": "string",
  //           "enum": [
  //             "5a8bb659-2472-4d89-5d12-08d907680c53"
  //           ],
  //           "title": "Alova 76"
  //         },
  //         {
  //           "type": "string",
  //           "enum": [
  //             "97ba1a89-bb3e-4321-5d13-08d907680c53"
  //           ],
  //           "title": "Alova 79"
  //         },
  //         {
  //           "type": "string",
  //           "enum": [
  //             "17f84b58-afee-4d4d-5d14-08d907680c53"
  //           ],
  //           "title": "Alova Pdp"
  //         },
  //         {
  //           "type": "string",
  //           "enum": [
  //             "1881bdd1-9000-4189-5d15-08d907680c53"
  //           ],
  //           "title": "Arte 51B"
  //         },
  //         {
  //           "type": "string",
  //           "enum": [
  //             "1dcb64d6-c365-402f-5d16-08d907680c53"
  //           ],
  //           "title": "Arte 80A"
  //         },
  //         {
  //           "type": "string",
  //           "enum": [
  //             "20993229-6bb7-43f9-5d17-08d907680c53"
  //           ],
  //           "title": "Berlin 01"
  //         },
  //         {
  //           "type": "string",
  //           "enum": [
  //             "08030dcf-7202-46b0-5d18-08d907680c53"
  //           ],
  //           "title": "Berlin 02"
  //         },
  //         {
  //           "type": "string",
  //           "enum": [
  //             "fad075c5-33dd-4ec5-5d19-08d907680c53"
  //           ],
  //           "title": "Berlin 03"
  //         },
  //         {
  //           "type": "string",
  //           "enum": [
  //             "c9e7d336-3572-4905-5d1a-08d907680c53"
  //           ],
  //           "title": "Berlin 10"
  //         },
  //         {
  //           "type": "string",
  //           "enum": [
  //             "f0af843d-3027-46bb-5d1b-08d907680c53"
  //           ],
  //           "title": "Beton"
  //         },
  //         {
  //           "type": "string",
  //           "enum": [
  //             "08b41797-3b50-4cfd-5d1c-08d907680c53"
  //           ],
  //           "title": "Botanical 80"
  //         },
  //         {
  //           "type": "string",
  //           "enum": [
  //             "02f84fc5-978c-4693-5d1d-08d907680c53"
  //           ],
  //           "title": "Butterfly 04"
  //         },
  //         {
  //           "type": "string",
  //           "enum": [
  //             "f83aae61-d285-4d86-5d1e-08d907680c53"
  //           ],
  //           "title": "Cover 02"
  //         },
  //         {
  //           "type": "string",
  //           "enum": [
  //             "a038b50b-32fc-4e57-5d1f-08d907680c53"
  //           ],
  //           "title": "Cover 61"
  //         },
  //         {
  //           "type": "string",
  //           "enum": [
  //             "dc3eec1e-3779-450b-5d20-08d907680c53"
  //           ],
  //           "title": "Cover 70"
  //         },
  //         {
  //           "type": "string",
  //           "enum": [
  //             "13fe5bc0-9cf8-4a16-5d21-08d907680c53"
  //           ],
  //           "title": "Cover 83"
  //         },
  //         {
  //           "type": "string",
  //           "enum": [
  //             "40bc9ff6-657a-4307-5d22-08d907680c53"
  //           ],
  //           "title": "Cover 87"
  //         },
  //         {
  //           "type": "string",
  //           "enum": [
  //             "1f3387a5-3f07-4686-5d23-08d907680c53"
  //           ],
  //           "title": "Dora 21"
  //         },
  //         {
  //           "type": "string",
  //           "enum": [
  //             "614e364d-762e-494b-5d24-08d907680c53"
  //           ],
  //           "title": "Dora 22"
  //         },
  //         {
  //           "type": "string",
  //           "enum": [
  //             "a6f5e6e9-14c4-48c6-5d25-08d907680c53"
  //           ],
  //           "title": "Dora 26"
  //         },
  //         {
  //           "type": "string",
  //           "enum": [
  //             "41c68412-1c72-4273-5d26-08d907680c53"
  //           ],
  //           "title": "Dora 28"
  //         },
  //         {
  //           "type": "string",
  //           "enum": [
  //             "34357bfe-667b-4f05-5d27-08d907680c53"
  //           ],
  //           "title": "Dora 63"
  //         },
  //         {
  //           "type": "string",
  //           "enum": [
  //             "95800e44-9857-4ca4-5d28-08d907680c53"
  //           ],
  //           "title": "Dora 85"
  //         },
  //         {
  //           "type": "string",
  //           "enum": [
  //             "9d78f087-7b07-4df0-5d29-08d907680c53"
  //           ],
  //           "title": "Dora 90"
  //         },
  //         {
  //           "type": "string",
  //           "enum": [
  //             "82e8e5de-181c-433f-5d2a-08d907680c53"
  //           ],
  //           "title": "Dora 95"
  //         },
  //         {
  //           "type": "string",
  //           "enum": [
  //             "a859b110-2042-421c-5d2b-08d907680c53"
  //           ],
  //           "title": "Dora 96"
  //         },
  //         {
  //           "type": "string",
  //           "enum": [
  //             "c70592b1-a781-42ee-5d2c-08d907680c53"
  //           ],
  //           "title": "Garden 39"
  //         },
  //         {
  //           "type": "string",
  //           "enum": [
  //             "655d8f6b-cc79-4c06-5d2d-08d907680c53"
  //           ],
  //           "title": "Glamour 38"
  //         },
  //         {
  //           "type": "string",
  //           "enum": [
  //             "1b8db6e8-6e07-477b-5d2e-08d907680c53"
  //           ],
  //           "title": "Grande 39"
  //         },
  //         {
  //           "type": "string",
  //           "enum": [
  //             "4d40f181-af9b-443b-5d2f-08d907680c53"
  //           ],
  //           "title": "Grande 75"
  //         },
  //         {
  //           "type": "string",
  //           "enum": [
  //             "9ad212b9-b629-4e3a-5d30-08d907680c53"
  //           ],
  //           "title": "Grande 77"
  //         },
  //         {
  //           "type": "string",
  //           "enum": [
  //             "d09065ea-dcce-470c-5d31-08d907680c53"
  //           ],
  //           "title": "Grande 81"
  //         },
  //         {
  //           "type": "string",
  //           "enum": [
  //             "53ed230c-9f13-4e5a-5d32-08d907680c53"
  //           ],
  //           "title": "Grande 97"
  //         },
  //         {
  //           "type": "string",
  //           "enum": [
  //             "de9806be-16d6-4269-5d33-08d907680c53"
  //           ],
  //           "title": "Gusto 61"
  //         },
  //         {
  //           "type": "string",
  //           "enum": [
  //             "06595ca9-f6fb-407f-5d34-08d907680c53"
  //           ],
  //           "title": "Gusto 69"
  //         },
  //         {
  //           "type": "string",
  //           "enum": [
  //             "2a97d04a-bbfc-47ac-5d35-08d907680c53"
  //           ],
  //           "title": "Gusto 82"
  //         },
  //         {
  //           "type": "string",
  //           "enum": [
  //             "5fd9086e-2aa0-442b-5d36-08d907680c53"
  //           ],
  //           "title": "Gusto 88"
  //         },
  //         {
  //           "type": "string",
  //           "enum": [
  //             "fd964a87-b2a2-43f0-5d37-08d907680c53"
  //           ],
  //           "title": "Inari 100"
  //         },
  //         {
  //           "type": "string",
  //           "enum": [
  //             "b697fad8-f097-4c76-5d38-08d907680c53"
  //           ],
  //           "title": "Inari 23"
  //         },
  //         {
  //           "type": "string",
  //           "enum": [
  //             "20788393-4c6a-4a89-5d39-08d907680c53"
  //           ],
  //           "title": "Inari 28"
  //         },
  //         {
  //           "type": "string",
  //           "enum": [
  //             "966fb645-f638-4501-5d3a-08d907680c53"
  //           ],
  //           "title": "Inari 80"
  //         },
  //         {
  //           "type": "string",
  //           "enum": [
  //             "a4dd4352-1d85-45db-5d3b-08d907680c53"
  //           ],
  //           "title": "Inari 91"
  //         },
  //         {
  //           "type": "string",
  //           "enum": [
  //             "16c9b360-237e-462e-5d3c-08d907680c53"
  //           ],
  //           "title": "Inari 96"
  //         },
  //         {
  //           "type": "string",
  //           "enum": [
  //             "166d9331-f306-49b3-5d3d-08d907680c53"
  //           ],
  //           "title": "Jasmine 22"
  //         },
  //         {
  //           "type": "string",
  //           "enum": [
  //             "5ba99299-3e6f-48a7-5d3e-08d907680c53"
  //           ],
  //           "title": "Jasmine 29"
  //         },
  //         {
  //           "type": "string",
  //           "enum": [
  //             "5426ff63-d468-40ce-5d3f-08d907680c53"
  //           ],
  //           "title": "Jasmine 73"
  //         },
  //         {
  //           "type": "string",
  //           "enum": [
  //             "8ef0116a-8557-4bd3-5d40-08d907680c53"
  //           ],
  //           "title": "Jasmine 85"
  //         },
  //         {
  //           "type": "string",
  //           "enum": [
  //             "4b8a9e95-411f-49ec-5d41-08d907680c53"
  //           ],
  //           "title": "Jasmine 90"
  //         },
  //         {
  //           "type": "string",
  //           "enum": [
  //             "026ea0d7-cc7f-4d6a-5d42-08d907680c53"
  //           ],
  //           "title": "Jasmine 96"
  //         },
  //         {
  //           "type": "string",
  //           "enum": [
  //             "18b78c52-2fc5-4f16-5d43-08d907680c53"
  //           ],
  //           "title": "Jungle 32"
  //         },
  //         {
  //           "type": "string",
  //           "enum": [
  //             "c9bb84f1-81ca-4ca7-5d44-08d907680c53"
  //           ],
  //           "title": "Jungle 37"
  //         },
  //         {
  //           "type": "string",
  //           "enum": [
  //             "edb6a3b0-dc7a-42b3-5d45-08d907680c53"
  //           ],
  //           "title": "Kronos 02"
  //         },
  //         {
  //           "type": "string",
  //           "enum": [
  //             "3da216d2-343b-4c2d-5d46-08d907680c53"
  //           ],
  //           "title": "Kronos 06"
  //         },
  //         {
  //           "type": "string",
  //           "enum": [
  //             "90c69a0e-d995-483c-5d47-08d907680c53"
  //           ],
  //           "title": "Kronos 07"
  //         },
  //         {
  //           "type": "string",
  //           "enum": [
  //             "2c3a83b2-472b-4c33-5d48-08d907680c53"
  //           ],
  //           "title": "Kronos 09"
  //         },
  //         {
  //           "type": "string",
  //           "enum": [
  //             "5926fb8e-75c4-4590-5d49-08d907680c53"
  //           ],
  //           "title": "Kronos 13"
  //         },
  //         {
  //           "type": "string",
  //           "enum": [
  //             "52f1d0f1-c343-44ea-5d4a-08d907680c53"
  //           ],
  //           "title": "Kronos 17"
  //         },
  //         {
  //           "type": "string",
  //           "enum": [
  //             "5a5a0eb9-be87-47f5-5d4b-08d907680c53"
  //           ],
  //           "title": "Kronos 19"
  //         },
  //         {
  //           "type": "string",
  //           "enum": [
  //             "cb74f2d5-e0e4-4ed2-5d4c-08d907680c53"
  //           ],
  //           "title": "Kronos 29"
  //         },
  //         {
  //           "type": "string",
  //           "enum": [
  //             "43e965c5-218b-468d-5d4d-08d907680c53"
  //           ],
  //           "title": "Lars 68"
  //         },
  //         {
  //           "type": "string",
  //           "enum": [
  //             "d85ce57b-17bc-43bf-5d4e-08d907680c53"
  //           ],
  //           "title": "Lars 90"
  //         },
  //         {
  //           "type": "string",
  //           "enum": [
  //             "c630e276-6420-4648-5d4f-08d907680c53"
  //           ],
  //           "title": "Lars 98"
  //         },
  //         {
  //           "type": "string",
  //           "enum": [
  //             "40061cc2-d471-4aa2-5d50-08d907680c53"
  //           ],
  //           "title": "Lars 99"
  //         },
  //         {
  //           "type": "string",
  //           "enum": [
  //             "ee0b0895-4453-4b9b-5d51-08d907680c53"
  //           ],
  //           "title": "Lastrico 2_76"
  //         },
  //         {
  //           "type": "string",
  //           "enum": [
  //             "bf19e830-b4ff-45cb-5d52-08d907680c53"
  //           ],
  //           "title": "Lima 67"
  //         },
  //         {
  //           "type": "string",
  //           "enum": [
  //             "1d31ca44-7ff8-44e8-5d53-08d907680c53"
  //           ],
  //           "title": "Lima 75"
  //         },
  //         {
  //           "type": "string",
  //           "enum": [
  //             "48836904-dac3-44be-5d54-08d907680c53"
  //           ],
  //           "title": "Madison 3_79"
  //         },
  //         {
  //           "type": "string",
  //           "enum": [
  //             "081a1fa5-bb28-44b2-5d55-08d907680c53"
  //           ],
  //           "title": "Malmo 05"
  //         },
  //         {
  //           "type": "string",
  //           "enum": [
  //             "92464d3d-2966-4dba-5d56-08d907680c53"
  //           ],
  //           "title": "Malmo 23"
  //         },
  //         {
  //           "type": "string",
  //           "enum": [
  //             "affd9e10-08e1-4896-5d57-08d907680c53"
  //           ],
  //           "title": "Malmo 26"
  //         },
  //         {
  //           "type": "string",
  //           "enum": [
  //             "bfe74368-6130-4aa5-5d58-08d907680c53"
  //           ],
  //           "title": "Malmo 37"
  //         },
  //         {
  //           "type": "string",
  //           "enum": [
  //             "9794cfc6-c5b2-4f55-5d59-08d907680c53"
  //           ],
  //           "title": "Malmo 41"
  //         },
  //         {
  //           "type": "string",
  //           "enum": [
  //             "b7dafa83-3c16-4e2b-5d5a-08d907680c53"
  //           ],
  //           "title": "Malmo 61"
  //         },
  //         {
  //           "type": "string",
  //           "enum": [
  //             "f5ea3bf7-d973-4c78-5d5b-08d907680c53"
  //           ],
  //           "title": "Malmo 63"
  //         },
  //         {
  //           "type": "string",
  //           "enum": [
  //             "98575b70-e1fe-4e25-5d5c-08d907680c53"
  //           ],
  //           "title": "Malmo 72"
  //         },
  //         {
  //           "type": "string",
  //           "enum": [
  //             "47d34bb0-5fa2-4b7d-5d5d-08d907680c53"
  //           ],
  //           "title": "Malmo 79"
  //         },
  //         {
  //           "type": "string",
  //           "enum": [
  //             "de8fa923-a3d5-426a-5d5e-08d907680c53"
  //           ],
  //           "title": "Malmo 81"
  //         },
  //         {
  //           "type": "string",
  //           "enum": [
  //             "369a0972-2f9a-49b0-5d5f-08d907680c53"
  //           ],
  //           "title": "Malmo 83"
  //         },
  //         {
  //           "type": "string",
  //           "enum": [
  //             "2973f06d-5a6b-4b8a-5d60-08d907680c53"
  //           ],
  //           "title": "Malmo 85"
  //         },
  //         {
  //           "type": "string",
  //           "enum": [
  //             "6451c00a-b16e-40cb-5d61-08d907680c53"
  //           ],
  //           "title": "Malmo 90"
  //         },
  //         {
  //           "type": "string",
  //           "enum": [
  //             "5694ede0-ff23-41f8-5d62-08d907680c53"
  //           ],
  //           "title": "Malmo 95"
  //         },
  //         {
  //           "type": "string",
  //           "enum": [
  //             "32a05bf0-0325-4015-5d63-08d907680c53"
  //           ],
  //           "title": "Malmo 96"
  //         },
  //         {
  //           "type": "string",
  //           "enum": [
  //             "898e2eb0-bc05-45f9-5d64-08d907680c53"
  //           ],
  //           "title": "Mat Velvet 29"
  //         },
  //         {
  //           "type": "string",
  //           "enum": [
  //             "cea49add-4125-4b7c-5d65-08d907680c53"
  //           ],
  //           "title": "Mat Velvet 63"
  //         },
  //         {
  //           "type": "string",
  //           "enum": [
  //             "46e96de2-fe77-4dee-5d66-08d907680c53"
  //           ],
  //           "title": "Mat Velvet 68"
  //         },
  //         {
  //           "type": "string",
  //           "enum": [
  //             "2e43123f-9dc6-4f6e-5d67-08d907680c53"
  //           ],
  //           "title": "Mat Velvet 75"
  //         },
  //         {
  //           "type": "string",
  //           "enum": [
  //             "60bfc3cc-6f58-49d2-5d68-08d907680c53"
  //           ],
  //           "title": "Mat Velvet 85"
  //         },
  //         {
  //           "type": "string",
  //           "enum": [
  //             "3fa1cde2-380a-434a-5d69-08d907680c53"
  //           ],
  //           "title": "Mat Velvet 99"
  //         },
  //         {
  //           "type": "string",
  //           "enum": [
  //             "4aea5dcf-4278-4527-5d6a-08d907680c53"
  //           ],
  //           "title": "Microfibre"
  //         },
  //         {
  //           "type": "string",
  //           "enum": [
  //             "a433823c-572f-4065-5d6b-08d907680c53"
  //           ],
  //           "title": "Monet 95"
  //         },
  //         {
  //           "type": "string",
  //           "enum": [
  //             "dba89dd2-424f-44c1-5d6c-08d907680c53"
  //           ],
  //           "title": "Monolith 09"
  //         },
  //         {
  //           "type": "string",
  //           "enum": [
  //             "e06137f7-8486-4efc-5d6d-08d907680c53"
  //           ],
  //           "title": "Monolith 29"
  //         },
  //         {
  //           "type": "string",
  //           "enum": [
  //             "a1a42d06-6af3-4d0f-5d6e-08d907680c53"
  //           ],
  //           "title": "Monolith 37"
  //         },
  //         {
  //           "type": "string",
  //           "enum": [
  //             "dc7b199d-6ed7-44d3-5d6f-08d907680c53"
  //           ],
  //           "title": "Monolith 38"
  //         },
  //         {
  //           "type": "string",
  //           "enum": [
  //             "86df96fc-9370-4a80-5d70-08d907680c53"
  //           ],
  //           "title": "Monolith 48"
  //         },
  //         {
  //           "type": "string",
  //           "enum": [
  //             "c25c8b4d-0ab8-4788-5d71-08d907680c53"
  //           ],
  //           "title": "Monolith 63"
  //         },
  //         {
  //           "type": "string",
  //           "enum": [
  //             "dc09257d-19a5-476b-5d72-08d907680c53"
  //           ],
  //           "title": "Monolith 77"
  //         },
  //         {
  //           "type": "string",
  //           "enum": [
  //             "c14b0090-b1f3-4b07-5d73-08d907680c53"
  //           ],
  //           "title": "Monolith 83"
  //         },
  //         {
  //           "type": "string",
  //           "enum": [
  //             "37b28f72-7949-4942-5d74-08d907680c53"
  //           ],
  //           "title": "Monolith 84"
  //         },
  //         {
  //           "type": "string",
  //           "enum": [
  //             "7d4558f2-012e-4514-5d75-08d907680c53"
  //           ],
  //           "title": "Monolith 85"
  //         },
  //         {
  //           "type": "string",
  //           "enum": [
  //             "9415fddc-5e69-4fea-5d76-08d907680c53"
  //           ],
  //           "title": "Monolith 97"
  //         },
  //         {
  //           "type": "string",
  //           "enum": [
  //             "734ea502-31ed-4dee-5d77-08d907680c53"
  //           ],
  //           "title": "Nubuk 11"
  //         },
  //         {
  //           "type": "string",
  //           "enum": [
  //             "4dc64eeb-f0d9-4281-5d78-08d907680c53"
  //           ],
  //           "title": "Nubuk 132"
  //         },
  //         {
  //           "type": "string",
  //           "enum": [
  //             "57f7ce79-018e-4305-5d79-08d907680c53"
  //           ],
  //           "title": "Nubuk 16"
  //         },
  //         {
  //           "type": "string",
  //           "enum": [
  //             "96590b06-a926-4743-5d7a-08d907680c53"
  //           ],
  //           "title": "Nubuk 2014"
  //         },
  //         {
  //           "type": "string",
  //           "enum": [
  //             "25d67d08-4103-4bf6-5d7b-08d907680c53"
  //           ],
  //           "title": "Nubuk 21"
  //         },
  //         {
  //           "type": "string",
  //           "enum": [
  //             "b21c5d58-2807-4ba0-5d7c-08d907680c53"
  //           ],
  //           "title": "Nubuk 27"
  //         },
  //         {
  //           "type": "string",
  //           "enum": [
  //             "1845252b-9d2b-4ec4-5d7d-08d907680c53"
  //           ],
  //           "title": "Nubuk 66"
  //         },
  //         {
  //           "type": "string",
  //           "enum": [
  //             "e88e05a4-4e90-40bc-5d7e-08d907680c53"
  //           ],
  //           "title": "Nut Burgundia"
  //         },
  //         {
  //           "type": "string",
  //           "enum": [
  //             "128a083d-fe90-49e9-5d7f-08d907680c53"
  //           ],
  //           "title": "Omega 02"
  //         },
  //         {
  //           "type": "string",
  //           "enum": [
  //             "df894dd4-5500-4e01-5d80-08d907680c53"
  //           ],
  //           "title": "Omega 13"
  //         },
  //         {
  //           "type": "string",
  //           "enum": [
  //             "5adbd0bd-1664-4ef5-5d81-08d907680c53"
  //           ],
  //           "title": "Omega 68"
  //         },
  //         {
  //           "type": "string",
  //           "enum": [
  //             "f60d339b-7c28-4704-5d82-08d907680c53"
  //           ],
  //           "title": "Omega 86"
  //         },
  //         {
  //           "type": "string",
  //           "enum": [
  //             "9196e218-908d-456b-5d83-08d907680c53"
  //           ],
  //           "title": "Omega 91"
  //         },
  //         {
  //           "type": "string",
  //           "enum": [
  //             "d70f8744-8c41-4fe0-5d84-08d907680c53"
  //           ],
  //           "title": "Ontario 100"
  //         },
  //         {
  //           "type": "string",
  //           "enum": [
  //             "b36308bf-61b6-43c3-5d85-08d907680c53"
  //           ],
  //           "title": "Ontario 22"
  //         },
  //         {
  //           "type": "string",
  //           "enum": [
  //             "3f6d9261-85b4-468a-5d86-08d907680c53"
  //           ],
  //           "title": "Ontario 40"
  //         },
  //         {
  //           "type": "string",
  //           "enum": [
  //             "c04afa04-bc05-4839-5d87-08d907680c53"
  //           ],
  //           "title": "Ontario 65"
  //         },
  //         {
  //           "type": "string",
  //           "enum": [
  //             "551f012c-c610-42bb-5d88-08d907680c53"
  //           ],
  //           "title": "Ontario 81"
  //         },
  //         {
  //           "type": "string",
  //           "enum": [
  //             "4f930859-4f28-4e6e-5d89-08d907680c53"
  //           ],
  //           "title": "Ontario 83"
  //         },
  //         {
  //           "type": "string",
  //           "enum": [
  //             "5cc8683e-a6cb-4585-5d8a-08d907680c53"
  //           ],
  //           "title": "Ontario 91"
  //         },
  //         {
  //           "type": "string",
  //           "enum": [
  //             "486c8066-e3a0-4f24-5d8b-08d907680c53"
  //           ],
  //           "title": "Ontario 96"
  //         },
  //         {
  //           "type": "string",
  //           "enum": [
  //             "3f0c48dc-6a39-4258-5d8c-08d907680c53"
  //           ],
  //           "title": "Orinoco 100"
  //         },
  //         {
  //           "type": "string",
  //           "enum": [
  //             "27e52887-b621-4574-5d8d-08d907680c53"
  //           ],
  //           "title": "Orinoco 21"
  //         },
  //         {
  //           "type": "string",
  //           "enum": [
  //             "9c74fccd-18e0-4c10-5d8e-08d907680c53"
  //           ],
  //           "title": "Orinoco 22"
  //         },
  //         {
  //           "type": "string",
  //           "enum": [
  //             "612ce0d2-6745-4b37-5d8f-08d907680c53"
  //           ],
  //           "title": "Orinoco 29"
  //         },
  //         {
  //           "type": "string",
  //           "enum": [
  //             "7e2dfeaa-6340-4ce4-5d90-08d907680c53"
  //           ],
  //           "title": "Orinoco 65"
  //         },
  //         {
  //           "type": "string",
  //           "enum": [
  //             "b7216bff-b87a-408b-5d91-08d907680c53"
  //           ],
  //           "title": "Orinoco 80"
  //         },
  //         {
  //           "type": "string",
  //           "enum": [
  //             "1f1d23ae-3165-4d72-5d92-08d907680c53"
  //           ],
  //           "title": "Orinoco 85"
  //         },
  //         {
  //           "type": "string",
  //           "enum": [
  //             "933fc81b-a251-4d9f-5d93-08d907680c53"
  //           ],
  //           "title": "Orinoco 96"
  //         },
  //         {
  //           "type": "string",
  //           "enum": [
  //             "aefc5610-f6d0-4b31-5d94-08d907680c53"
  //           ],
  //           "title": "Palacio 77"
  //         },
  //         {
  //           "type": "string",
  //           "enum": [
  //             "66371cd9-ab15-4768-5d95-08d907680c53"
  //           ],
  //           "title": "Paros 02"
  //         },
  //         {
  //           "type": "string",
  //           "enum": [
  //             "2cb6e59c-93a8-46b0-5d96-08d907680c53"
  //           ],
  //           "title": "Paros 05"
  //         },
  //         {
  //           "type": "string",
  //           "enum": [
  //             "d7034190-bb78-4a3a-5d97-08d907680c53"
  //           ],
  //           "title": "Paros 06"
  //         },
  //         {
  //           "type": "string",
  //           "enum": [
  //             "ead5870b-5564-48f4-5d98-08d907680c53"
  //           ],
  //           "title": "Portland 90"
  //         },
  //         {
  //           "type": "string",
  //           "enum": [
  //             "3407c521-f16a-4edc-5d99-08d907680c53"
  //           ],
  //           "title": "Portland 95"
  //         },
  //         {
  //           "type": "string",
  //           "enum": [
  //             "e7043f02-4601-40d9-5d9a-08d907680c53"
  //           ],
  //           "title": "Primo 89"
  //         },
  //         {
  //           "type": "string",
  //           "enum": [
  //             "e88e66c0-e271-401d-5d9b-08d907680c53"
  //           ],
  //           "title": "Primo 96"
  //         },
  //         {
  //           "type": "string",
  //           "enum": [
  //             "ca810ae5-fb8f-4651-5d9c-08d907680c53"
  //           ],
  //           "title": "Rivera 100"
  //         },
  //         {
  //           "type": "string",
  //           "enum": [
  //             "a06bf8e4-1fcb-48ca-5d9d-08d907680c53"
  //           ],
  //           "title": "Rivera 36"
  //         },
  //         {
  //           "type": "string",
  //           "enum": [
  //             "1d7d9417-623a-4b9e-5d9e-08d907680c53"
  //           ],
  //           "title": "Rivera 59"
  //         },
  //         {
  //           "type": "string",
  //           "enum": [
  //             "9e3041e4-ebc9-45d8-5d9f-08d907680c53"
  //           ],
  //           "title": "Rose 14"
  //         },
  //         {
  //           "type": "string",
  //           "enum": [
  //             "b13d09f5-4dc9-49b9-5da0-08d907680c53"
  //           ],
  //           "title": "Sawana 01"
  //         },
  //         {
  //           "type": "string",
  //           "enum": [
  //             "55a8f9a0-b80d-4d02-5da1-08d907680c53"
  //           ],
  //           "title": "Sawana 05"
  //         },
  //         {
  //           "type": "string",
  //           "enum": [
  //             "1c4de198-e667-4f80-5da2-08d907680c53"
  //           ],
  //           "title": "Sawana 14"
  //         },
  //         {
  //           "type": "string",
  //           "enum": [
  //             "3cab3045-ef37-4d2c-5da3-08d907680c53"
  //           ],
  //           "title": "Sawana 16"
  //         },
  //         {
  //           "type": "string",
  //           "enum": [
  //             "a45d7590-61bc-4d74-5da4-08d907680c53"
  //           ],
  //           "title": "Sawana 21"
  //         },
  //         {
  //           "type": "string",
  //           "enum": [
  //             "4c656fde-d127-4d6a-5da5-08d907680c53"
  //           ],
  //           "title": "Sawana 25"
  //         },
  //         {
  //           "type": "string",
  //           "enum": [
  //             "d6bb9ce9-24d5-40d8-5da6-08d907680c53"
  //           ],
  //           "title": "Sawana 26"
  //         },
  //         {
  //           "type": "string",
  //           "enum": [
  //             "0f8d91b2-aa6f-4701-5da7-08d907680c53"
  //           ],
  //           "title": "Sawana 72"
  //         },
  //         {
  //           "type": "string",
  //           "enum": [
  //             "28360fe1-c59b-40c4-5da8-08d907680c53"
  //           ],
  //           "title": "Sawana 80"
  //         },
  //         {
  //           "type": "string",
  //           "enum": [
  //             "3ddc9e75-1c3b-4a95-5da9-08d907680c53"
  //           ],
  //           "title": "Sawana 84"
  //         },
  //         {
  //           "type": "string",
  //           "enum": [
  //             "047ac50b-ebab-461b-5daa-08d907680c53"
  //           ],
  //           "title": "Soft 09"
  //         },
  //         {
  //           "type": "string",
  //           "enum": [
  //             "a6148363-bff1-459d-5dab-08d907680c53"
  //           ],
  //           "title": "Soft 10"
  //         },
  //         {
  //           "type": "string",
  //           "enum": [
  //             "de720e8c-b567-4cf1-5dac-08d907680c53"
  //           ],
  //           "title": "Soft 11"
  //         },
  //         {
  //           "type": "string",
  //           "enum": [
  //             "be6f482b-b31b-4b82-5dad-08d907680c53"
  //           ],
  //           "title": "Soft 15"
  //         },
  //         {
  //           "type": "string",
  //           "enum": [
  //             "e40fcb84-2c83-45d1-5dae-08d907680c53"
  //           ],
  //           "title": "Soft 16"
  //         },
  //         {
  //           "type": "string",
  //           "enum": [
  //             "ff2923e8-cb75-4d9c-5daf-08d907680c53"
  //           ],
  //           "title": "Soft 17"
  //         },
  //         {
  //           "type": "string",
  //           "enum": [
  //             "ab3019f8-391a-44c0-5db0-08d907680c53"
  //           ],
  //           "title": "Soft 18"
  //         },
  //         {
  //           "type": "string",
  //           "enum": [
  //             "96e70345-9cad-4a4a-5db1-08d907680c53"
  //           ],
  //           "title": "Soft 29"
  //         },
  //         {
  //           "type": "string",
  //           "enum": [
  //             "ed4ceef8-48d1-46ae-5db2-08d907680c53"
  //           ],
  //           "title": "Soft 33"
  //         },
  //         {
  //           "type": "string",
  //           "enum": [
  //             "3dc0ba9b-e769-4996-5db3-08d907680c53"
  //           ],
  //           "title": "Soft 66"
  //         },
  //         {
  //           "type": "string",
  //           "enum": [
  //             "71a4ff0e-c895-45cc-5db4-08d907680c53"
  //           ],
  //           "title": "Soft 929"
  //         },
  //         {
  //           "type": "string",
  //           "enum": [
  //             "f9535ff5-e33d-4128-5db5-08d907680c53"
  //           ],
  //           "title": "Soft 985"
  //         },
  //         {
  //           "type": "string",
  //           "enum": [
  //             "ad923835-eb19-4e29-5db6-08d907680c53"
  //           ],
  //           "title": "Solar 16"
  //         },
  //         {
  //           "type": "string",
  //           "enum": [
  //             "151e41c9-a8b5-4a88-5db7-08d907680c53"
  //           ],
  //           "title": "Solar 63"
  //         },
  //         {
  //           "type": "string",
  //           "enum": [
  //             "b9cfe2ca-6f44-4644-5db8-08d907680c53"
  //           ],
  //           "title": "Solar 70"
  //         },
  //         {
  //           "type": "string",
  //           "enum": [
  //             "f4a22e68-a15a-4657-5db9-08d907680c53"
  //           ],
  //           "title": "Solar 79"
  //         },
  //         {
  //           "type": "string",
  //           "enum": [
  //             "130cbe5f-8fac-49aa-5dba-08d907680c53"
  //           ],
  //           "title": "Solar 80"
  //         },
  //         {
  //           "type": "string",
  //           "enum": [
  //             "3f2cd8a9-67cf-4165-5dbb-08d907680c53"
  //           ],
  //           "title": "Solar 99"
  //         },
  //         {
  //           "type": "string",
  //           "enum": [
  //             "78469ebf-8847-4ee2-5dbc-08d907680c53"
  //           ],
  //           "title": "Solid 09"
  //         },
  //         {
  //           "type": "string",
  //           "enum": [
  //             "930f3c1e-5e39-45dd-5dbd-08d907680c53"
  //           ],
  //           "title": "Solid 39"
  //         },
  //         {
  //           "type": "string",
  //           "enum": [
  //             "ccd6c461-6b26-4430-5dbe-08d907680c53"
  //           ],
  //           "title": "Solid 77"
  //         },
  //         {
  //           "type": "string",
  //           "enum": [
  //             "8e94ad50-afc5-42bd-5dbf-08d907680c53"
  //           ],
  //           "title": "Sonoma"
  //         },
  //         {
  //           "type": "string",
  //           "enum": [
  //             "fca6ee1a-c86a-4a74-5dc0-08d907680c53"
  //           ],
  //           "title": "Soro 100"
  //         },
  //         {
  //           "type": "string",
  //           "enum": [
  //             "5e9d74b6-6a50-426e-5dc1-08d907680c53"
  //           ],
  //           "title": "Soro 13"
  //         },
  //         {
  //           "type": "string",
  //           "enum": [
  //             "d178fcbc-4554-4dd9-5dc2-08d907680c53"
  //           ],
  //           "title": "Soro 28"
  //         },
  //         {
  //           "type": "string",
  //           "enum": [
  //             "fe5652d0-7a05-44cd-5dc3-08d907680c53"
  //           ],
  //           "title": "Soro 34"
  //         },
  //         {
  //           "type": "string",
  //           "enum": [
  //             "bdc164fb-a581-4bca-5dc4-08d907680c53"
  //           ],
  //           "title": "Soro 40"
  //         },
  //         {
  //           "type": "string",
  //           "enum": [
  //             "863754b2-c45c-4ed3-5dc5-08d907680c53"
  //           ],
  //           "title": "Soro 61"
  //         },
  //         {
  //           "type": "string",
  //           "enum": [
  //             "33c663dd-7255-4d93-5dc6-08d907680c53"
  //           ],
  //           "title": "Soro 65"
  //         },
  //         {
  //           "type": "string",
  //           "enum": [
  //             "14a0738c-1c19-4c3a-5dc7-08d907680c53"
  //           ],
  //           "title": "Soro 76"
  //         },
  //         {
  //           "type": "string",
  //           "enum": [
  //             "84053ea5-3255-4194-5dc8-08d907680c53"
  //           ],
  //           "title": "Soro 83"
  //         },
  //         {
  //           "type": "string",
  //           "enum": [
  //             "6de95b94-febb-40db-5dc9-08d907680c53"
  //           ],
  //           "title": "Soro 93"
  //         },
  //         {
  //           "type": "string",
  //           "enum": [
  //             "566db046-0799-46a1-5dca-08d907680c53"
  //           ],
  //           "title": "Soro 95"
  //         },
  //         {
  //           "type": "string",
  //           "enum": [
  //             "53b862dd-1120-4039-5dcb-08d907680c53"
  //           ],
  //           "title": "Texas 23"
  //         },
  //         {
  //           "type": "string",
  //           "enum": [
  //             "148c1e52-a3e0-4e29-5dcc-08d907680c53"
  //           ],
  //           "title": "Texas 28"
  //         },
  //         {
  //           "type": "string",
  //           "enum": [
  //             "f2a27a6a-deab-4693-5dcd-08d907680c53"
  //           ],
  //           "title": "Texas 92"
  //         },
  //         {
  //           "type": "string",
  //           "enum": [
  //             "d7d5241f-ce0e-4c6d-5dce-08d907680c53"
  //           ],
  //           "title": "Vernal 61"
  //         },
  //         {
  //           "type": "string",
  //           "enum": [
  //             "e7d01d23-b9c1-40f5-5dcf-08d907680c53"
  //           ],
  //           "title": "White Mat"
  //         },
  //         {
  //           "type": "string",
  //           "enum": [
  //             "36da2a3a-3900-4f61-5dd0-08d907680c53"
  //           ],
  //           "title": "Zigzag 53"
  //         },
  //         {
  //           "type": "string",
  //           "enum": [
  //             "a7d4d8df-c526-44d5-5dd1-08d907680c53"
  //           ],
  //           "title": "Zigzag 60"
  //         },
  //         {
  //           "type": "string",
  //           "enum": [
  //             "ef1742c5-6155-4237-2346-08d91f9f2f88"
  //           ],
  //           "title": "Solar 77"
  //         },
  //         {
  //           "type": "string",
  //           "enum": [
  //             "0eb04c10-9804-4597-2347-08d91f9f2f88"
  //           ],
  //           "title": "Arte 89A"
  //         },
  //         {
  //           "type": "string",
  //           "enum": [
  //             "5573e5e8-3db2-4742-2348-08d91f9f2f88"
  //           ],
  //           "title": "Primo 88"
  //         },
  //         {
  //           "type": "string",
  //           "enum": [
  //             "90c6c26c-4b03-43b7-07dc-08d92f34c637"
  //           ],
  //           "title": "Solar 45"
  //         },
  //         {
  //           "type": "string",
  //           "enum": [
  //             "866add69-8cee-4c13-07dd-08d92f34c637"
  //           ],
  //           "title": "Texas 26"
  //         },
  //         {
  //           "type": "string",
  //           "enum": [
  //             "219d1818-7170-481e-07de-08d92f34c637"
  //           ],
  //           "title": "Riviera 38"
  //         },
  //         {
  //           "type": "string",
  //           "enum": [
  //             "20d46d2a-0b4e-48eb-07df-08d92f34c637"
  //           ],
  //           "title": "Solar 96"
  //         },
  //         {
  //           "type": "string",
  //           "enum": [
  //             "9501229c-0999-4de7-81cf-08d93b9e6dba"
  //           ],
  //           "title": "Palacio 79"
  //         },
  //         {
  //           "type": "string",
  //           "enum": [
  //             "8d8d8317-e956-4c5a-81d0-08d93b9e6dba"
  //           ],
  //           "title": "Arte80A"
  //         },
  //         {
  //           "type": "string",
  //           "enum": [
  //             "bb0f608d-2bde-4998-81d1-08d93b9e6dba"
  //           ],
  //           "title": "Monet 68"
  //         },
  //         {
  //           "type": "string",
  //           "enum": [
  //             "72762e7b-9d2a-42f8-81d2-08d93b9e6dba"
  //           ],
  //           "title": "Riviera 91"
  //         },
  //         {
  //           "type": "string",
  //           "enum": [
  //             "9ceeeb5f-3a3e-406a-81d3-08d93b9e6dba"
  //           ],
  //           "title": "Riviera 97"
  //         },
  //         {
  //           "type": "string",
  //           "enum": [
  //             "c3b61927-cbe2-4855-81d4-08d93b9e6dba"
  //           ],
  //           "title": "Kongo 195"
  //         },
  //         {
  //           "type": "string",
  //           "enum": [
  //             "5e3f06f4-5690-4157-81d5-08d93b9e6dba"
  //           ],
  //           "title": "Mat Velvet 79"
  //         },
  //         {
  //           "type": "string",
  //           "enum": [
  //             "23e5bd17-9dd4-4a5f-ce6c-08d95afcbf75"
  //           ],
  //           "title": "Grosso 22"
  //         },
  //         {
  //           "type": "string",
  //           "enum": [
  //             "319cb0af-3b8a-405b-ce6d-08d95afcbf75"
  //           ],
  //           "title": "Grosso 10"
  //         },
  //         {
  //           "type": "string",
  //           "enum": [
  //             "be108d92-6cd2-4966-ce6e-08d95afcbf75"
  //           ],
  //           "title": "Grosso 20"
  //         },
  //         {
  //           "type": "string",
  //           "enum": [
  //             "a1fc1237-fb99-4706-ce6f-08d95afcbf75"
  //           ],
  //           "title": "Grosso 18"
  //         },
  //         {
  //           "type": "string",
  //           "enum": [
  //             "86814129-7a02-476f-ce70-08d95afcbf75"
  //           ],
  //           "title": "Grosso 21"
  //         },
  //         {
  //           "type": "string",
  //           "enum": [
  //             "b28a736b-f2bd-4bda-ce71-08d95afcbf75"
  //           ],
  //           "title": "Grosso 4"
  //         },
  //         {
  //           "type": "string",
  //           "enum": [
  //             "76937650-c871-476f-ce72-08d95afcbf75"
  //           ],
  //           "title": "Grosso 38"
  //         },
  //         {
  //           "type": "string",
  //           "enum": [
  //             "3b385d22-4c2e-41a5-ce73-08d95afcbf75"
  //           ],
  //           "title": "Grosso 5"
  //         },
  //         {
  //           "type": "string",
  //           "enum": [
  //             "b9676ae6-75bc-4133-ce74-08d95afcbf75"
  //           ],
  //           "title": "Grosso 33"
  //         },
  //         {
  //           "type": "string",
  //           "enum": [
  //             "c944ef21-d4c3-4b39-ce75-08d95afcbf75"
  //           ],
  //           "title": "Grosso 48"
  //         },
  //         {
  //           "type": "string",
  //           "enum": [
  //             "98f7343f-7122-41da-ce76-08d95afcbf75"
  //           ],
  //           "title": "Grosso 30"
  //         },
  //         {
  //           "type": "string",
  //           "enum": [
  //             "45621f6a-0ed2-4ac3-ce77-08d95afcbf75"
  //           ],
  //           "title": "Grosso 6"
  //         },
  //         {
  //           "type": "string",
  //           "enum": [
  //             "aaa293ea-f038-4427-ce78-08d95afcbf75"
  //           ],
  //           "title": "Flores 5"
  //         },
  //         {
  //           "type": "string",
  //           "enum": [
  //             "5207bbaa-f617-4d66-ce79-08d95afcbf75"
  //           ],
  //           "title": "Flores 10"
  //         },
  //         {
  //           "type": "string",
  //           "enum": [
  //             "6b39ce3c-fd88-4041-ce7a-08d95afcbf75"
  //           ],
  //           "title": "Flores 21"
  //         },
  //         {
  //           "type": "string",
  //           "enum": [
  //             "fc0a66b0-9c9b-4764-ce7b-08d95afcbf75"
  //           ],
  //           "title": "Flores 4"
  //         },
  //         {
  //           "type": "string",
  //           "enum": [
  //             "11b630c1-f2a5-4c88-ce7c-08d95afcbf75"
  //           ],
  //           "title": "Flores 19"
  //         },
  //         {
  //           "type": "string",
  //           "enum": [
  //             "2d8694c5-126b-4b79-ce7d-08d95afcbf75"
  //           ],
  //           "title": "Flores 22"
  //         },
  //         {
  //           "type": "string",
  //           "enum": [
  //             "1f57e239-41bf-4d44-ce7e-08d95afcbf75"
  //           ],
  //           "title": "Flores 100"
  //         },
  //         {
  //           "type": "string",
  //           "enum": [
  //             "0f033e39-3b9e-4d4d-ce7f-08d95afcbf75"
  //           ],
  //           "title": "Flores 40"
  //         },
  //         {
  //           "type": "string",
  //           "enum": [
  //             "8fbb0ca0-a081-4062-ce83-08d95afcbf75"
  //           ],
  //           "title": "Fogo 19"
  //         },
  //         {
  //           "type": "string",
  //           "enum": [
  //             "5c55d57c-493f-4590-ce84-08d95afcbf75"
  //           ],
  //           "title": "Fogo 35"
  //         },
  //         {
  //           "type": "string",
  //           "enum": [
  //             "fc0c184b-6424-49c9-ce85-08d95afcbf75"
  //           ],
  //           "title": "Fogo 21"
  //         },
  //         {
  //           "type": "string",
  //           "enum": [
  //             "ea2a4168-ba2c-4d55-ce86-08d95afcbf75"
  //           ],
  //           "title": "Fogo 45"
  //         },
  //         {
  //           "type": "string",
  //           "enum": [
  //             "aee20dae-4dab-4e0a-ce87-08d95afcbf75"
  //           ],
  //           "title": "Fogo 101"
  //         },
  //         {
  //           "type": "string",
  //           "enum": [
  //             "11cbe524-e271-4746-ce88-08d95afcbf75"
  //           ],
  //           "title": "Fogo 100"
  //         },
  //         {
  //           "type": "string",
  //           "enum": [
  //             "59b4cda4-6d5e-4e51-ce89-08d95afcbf75"
  //           ],
  //           "title": "Fogo 40"
  //         },
  //         {
  //           "type": "string",
  //           "enum": [
  //             "13bfdfa2-1bf0-4172-ce8a-08d95afcbf75"
  //           ],
  //           "title": "Fogo 3"
  //         },
  //         {
  //           "type": "string",
  //           "enum": [
  //             "d7787b52-0c00-44b4-ce8b-08d95afcbf75"
  //           ],
  //           "title": "Fogo 38"
  //         },
  //         {
  //           "type": "string",
  //           "enum": [
  //             "244eb1ea-8413-440a-ce8c-08d95afcbf75"
  //           ],
  //           "title": "Fogo 4"
  //         },
  //         {
  //           "type": "string",
  //           "enum": [
  //             "833d9ea2-2de3-4c76-ce8d-08d95afcbf75"
  //           ],
  //           "title": "Fogo 5"
  //         },
  //         {
  //           "type": "string",
  //           "enum": [
  //             "345a95e8-bdd5-407b-ce8e-08d95afcbf75"
  //           ],
  //           "title": "Fogo 6"
  //         },
  //         {
  //           "type": "string",
  //           "enum": [
  //             "9f40a034-bf77-480d-ce8f-08d95afcbf75"
  //           ],
  //           "title": "Poco 7"
  //         },
  //         {
  //           "type": "string",
  //           "enum": [
  //             "d2ab4b98-a1f3-4f7b-ce90-08d95afcbf75"
  //           ],
  //           "title": "Poco 22"
  //         },
  //         {
  //           "type": "string",
  //           "enum": [
  //             "e2f27138-6fcc-4f40-ce91-08d95afcbf75"
  //           ],
  //           "title": "Poco 100"
  //         },
  //         {
  //           "type": "string",
  //           "enum": [
  //             "461be1f1-7670-480f-ce92-08d95afcbf75"
  //           ],
  //           "title": "Poco 50"
  //         },
  //         {
  //           "type": "string",
  //           "enum": [
  //             "3b38351c-176d-4f68-ce93-08d95afcbf75"
  //           ],
  //           "title": "Poco 101"
  //         },
  //         {
  //           "type": "string",
  //           "enum": [
  //             "b180d059-56e2-4cd1-ce94-08d95afcbf75"
  //           ],
  //           "title": "Poco 41"
  //         },
  //         {
  //           "type": "string",
  //           "enum": [
  //             "6159120e-be6b-4816-ce95-08d95afcbf75"
  //           ],
  //           "title": "Poco 40"
  //         },
  //         {
  //           "type": "string",
  //           "enum": [
  //             "375ded91-8c3f-466c-ce96-08d95afcbf75"
  //           ],
  //           "title": "Poco 3"
  //         },
  //         {
  //           "type": "string",
  //           "enum": [
  //             "bf964c21-8076-4b31-ce97-08d95afcbf75"
  //           ],
  //           "title": "Poco 4"
  //         },
  //         {
  //           "type": "string",
  //           "enum": [
  //             "c3443fbe-e9fe-4487-ce98-08d95afcbf75"
  //           ],
  //           "title": "Poco 5"
  //         },
  //         {
  //           "type": "string",
  //           "enum": [
  //             "ead106fa-6a7a-40bf-ce99-08d95afcbf75"
  //           ],
  //           "title": "Poco 6"
  //         },
  //         {
  //           "type": "string",
  //           "enum": [
  //             "353ce78e-79e8-4f0a-ce9a-08d95afcbf75"
  //           ],
  //           "title": "Poco 10"
  //         },
  //         {
  //           "type": "string",
  //           "enum": [
  //             "6c3a55a3-249c-47fa-ce9b-08d95afcbf75"
  //           ],
  //           "title": "Elba 1"
  //         },
  //         {
  //           "type": "string",
  //           "enum": [
  //             "2313bc7f-ffcf-4321-ce9c-08d95afcbf75"
  //           ],
  //           "title": "Elba 20"
  //         },
  //         {
  //           "type": "string",
  //           "enum": [
  //             "0980b079-101b-4c29-ce9d-08d95afcbf75"
  //           ],
  //           "title": "Elba 4"
  //         },
  //         {
  //           "type": "string",
  //           "enum": [
  //             "1bfa204b-df8c-43bf-ce9e-08d95afcbf75"
  //           ],
  //           "title": "Elba 5"
  //         },
  //         {
  //           "type": "string",
  //           "enum": [
  //             "2d14a4aa-d1bb-4a9f-ce9f-08d95afcbf75"
  //           ],
  //           "title": "Elba 41"
  //         },
  //         {
  //           "type": "string",
  //           "enum": [
  //             "361767d3-4c44-4552-cea0-08d95afcbf75"
  //           ],
  //           "title": "Elba 18"
  //         },
  //         {
  //           "type": "string",
  //           "enum": [
  //             "6bd8a3a8-5f20-4d7e-cea1-08d95afcbf75"
  //           ],
  //           "title": "Elba 10"
  //         },
  //         {
  //           "type": "string",
  //           "enum": [
  //             "8a7ef136-1c92-423c-cea2-08d95afcbf75"
  //           ],
  //           "title": "Vero 18"
  //         },
  //         {
  //           "type": "string",
  //           "enum": [
  //             "22c30cf4-9eb0-4a2d-cea3-08d95afcbf75"
  //           ],
  //           "title": "Vero 21"
  //         },
  //         {
  //           "type": "string",
  //           "enum": [
  //             "1d92a861-c987-4583-cea4-08d95afcbf75"
  //           ],
  //           "title": "Vero 40"
  //         },
  //         {
  //           "type": "string",
  //           "enum": [
  //             "8d4fc864-8d66-4bfe-cea5-08d95afcbf75"
  //           ],
  //           "title": "Vero 4"
  //         },
  //         {
  //           "type": "string",
  //           "enum": [
  //             "538a7d57-1d41-4b9b-cea6-08d95afcbf75"
  //           ],
  //           "title": "Vero 5"
  //         },
  //         {
  //           "type": "string",
  //           "enum": [
  //             "b8501ce9-24e8-404a-cea7-08d95afcbf75"
  //           ],
  //           "title": "Vero 10"
  //         },
  //         {
  //           "type": "string",
  //           "enum": [
  //             "cfbb16d5-e55f-492e-cea8-08d95afcbf75"
  //           ],
  //           "title": "Roko 101"
  //         },
  //         {
  //           "type": "string",
  //           "enum": [
  //             "0363b66e-beca-4f4b-cea9-08d95afcbf75"
  //           ],
  //           "title": "Roko 24"
  //         },
  //         {
  //           "type": "string",
  //           "enum": [
  //             "2bf6e465-3dc3-4886-ceaa-08d95afcbf75"
  //           ],
  //           "title": "Roko 4"
  //         },
  //         {
  //           "type": "string",
  //           "enum": [
  //             "a0cb8749-f366-482c-ceab-08d95afcbf75"
  //           ],
  //           "title": "Roko 5"
  //         },
  //         {
  //           "type": "string",
  //           "enum": [
  //             "bd63b2fb-4af5-43fe-ceac-08d95afcbf75"
  //           ],
  //           "title": "Tipa 18"
  //         },
  //         {
  //           "type": "string",
  //           "enum": [
  //             "31b8c88d-53d9-445f-cead-08d95afcbf75"
  //           ],
  //           "title": "Tipa 101"
  //         },
  //         {
  //           "type": "string",
  //           "enum": [
  //             "154b3046-2dac-4fdc-ceae-08d95afcbf75"
  //           ],
  //           "title": "Tipa 100"
  //         },
  //         {
  //           "type": "string",
  //           "enum": [
  //             "9b1aa38b-59a6-4699-ceaf-08d95afcbf75"
  //           ],
  //           "title": "Tipa 3"
  //         },
  //         {
  //           "type": "string",
  //           "enum": [
  //             "e7807d16-2171-426c-ceb0-08d95afcbf75"
  //           ],
  //           "title": "Tipa 4"
  //         },
  //         {
  //           "type": "string",
  //           "enum": [
  //             "314ce55e-c62d-413e-ceb1-08d95afcbf75"
  //           ],
  //           "title": "Orio 18"
  //         },
  //         {
  //           "type": "string",
  //           "enum": [
  //             "27b0711e-d3c1-4426-ceb2-08d95afcbf75"
  //           ],
  //           "title": "Orio 45"
  //         },
  //         {
  //           "type": "string",
  //           "enum": [
  //             "73274bc2-e5a6-452d-ceb3-08d95afcbf75"
  //           ],
  //           "title": "Orio 41"
  //         },
  //         {
  //           "type": "string",
  //           "enum": [
  //             "7e2e9193-c365-4774-ceb4-08d95afcbf75"
  //           ],
  //           "title": "Orio 40"
  //         },
  //         {
  //           "type": "string",
  //           "enum": [
  //             "319ae017-d7b9-4132-ceb5-08d95afcbf75"
  //           ],
  //           "title": "Orio 38"
  //         },
  //         {
  //           "type": "string",
  //           "enum": [
  //             "4d840bfd-bb80-4c3c-ceb6-08d95afcbf75"
  //           ],
  //           "title": "Orio 4"
  //         },
  //         {
  //           "type": "string",
  //           "enum": [
  //             "e1754c35-7419-41bf-ceb7-08d95afcbf75"
  //           ],
  //           "title": "Orio 5"
  //         },
  //         {
  //           "type": "string",
  //           "enum": [
  //             "66d0104e-9aff-4403-ceb8-08d95afcbf75"
  //           ],
  //           "title": "Orio 10"
  //         },
  //         {
  //           "type": "string",
  //           "enum": [
  //             "c7661898-68ac-4b11-ceb9-08d95afcbf75"
  //           ],
  //           "title": "Revo 41"
  //         },
  //         {
  //           "type": "string",
  //           "enum": [
  //             "398bcc41-5af1-47af-ceba-08d95afcbf75"
  //           ],
  //           "title": "Revo 4"
  //         },
  //         {
  //           "type": "string",
  //           "enum": [
  //             "797e8419-ad4c-4379-cebb-08d95afcbf75"
  //           ],
  //           "title": "Revo 5"
  //         },
  //         {
  //           "type": "string",
  //           "enum": [
  //             "1731bafd-5179-4690-cebc-08d95afcbf75"
  //           ],
  //           "title": "Revo 6"
  //         },
  //         {
  //           "type": "string",
  //           "enum": [
  //             "e62db34a-12b4-4e50-cebd-08d95afcbf75"
  //           ],
  //           "title": "Milda 4"
  //         },
  //         {
  //           "type": "string",
  //           "enum": [
  //             "9a9e353b-f5d6-4c07-cebe-08d95afcbf75"
  //           ],
  //           "title": "Milda 5"
  //         },
  //         {
  //           "type": "string",
  //           "enum": [
  //             "0fb9764b-8c3b-4dca-cebf-08d95afcbf75"
  //           ],
  //           "title": "Milda 6"
  //         },
  //         {
  //           "type": "string",
  //           "enum": [
  //             "5e4e11ca-621f-422f-cec0-08d95afcbf75"
  //           ],
  //           "title": "Gojo 4"
  //         },
  //         {
  //           "type": "string",
  //           "enum": [
  //             "f64ef595-7beb-43ec-cec1-08d95afcbf75"
  //           ],
  //           "title": "Gojo 5"
  //         },
  //         {
  //           "type": "string",
  //           "enum": [
  //             "7068985f-3b83-47f9-cec2-08d95afcbf75"
  //           ],
  //           "title": "Gojo 6"
  //         },
  //         {
  //           "type": "string",
  //           "enum": [
  //             "c3bdef36-04bd-4e2a-cec3-08d95afcbf75"
  //           ],
  //           "title": "Gojo 45"
  //         },
  //         {
  //           "type": "string",
  //           "enum": [
  //             "b6850ce6-0d51-4f22-cec4-08d95afcbf75"
  //           ],
  //           "title": "Gojo 33"
  //         },
  //         {
  //           "type": "string",
  //           "enum": [
  //             "22ded5cb-568c-42eb-cec5-08d95afcbf75"
  //           ],
  //           "title": "Gojo 40"
  //         },
  //         {
  //           "type": "string",
  //           "enum": [
  //             "e8ceab6c-2e8d-4aab-cec6-08d95afcbf75"
  //           ],
  //           "title": "Gojo 101"
  //         },
  //         {
  //           "type": "string",
  //           "enum": [
  //             "59e5e20a-8acf-4cc4-cec7-08d95afcbf75"
  //           ],
  //           "title": "Rekta 18"
  //         },
  //         {
  //           "type": "string",
  //           "enum": [
  //             "df879573-3a0d-4e76-cec8-08d95afcbf75"
  //           ],
  //           "title": "Rekta 22"
  //         },
  //         {
  //           "type": "string",
  //           "enum": [
  //             "c55da5ed-f178-409d-cec9-08d95afcbf75"
  //           ],
  //           "title": "Rekta 101"
  //         },
  //         {
  //           "type": "string",
  //           "enum": [
  //             "57636ef3-5c12-427b-ceca-08d95afcbf75"
  //           ],
  //           "title": "Rekta 100"
  //         },
  //         {
  //           "type": "string",
  //           "enum": [
  //             "bc1fdea5-fbb7-4474-cecb-08d95afcbf75"
  //           ],
  //           "title": "Rekta 38"
  //         },
  //         {
  //           "type": "string",
  //           "enum": [
  //             "abe6e6de-50f0-42d0-cecc-08d95afcbf75"
  //           ],
  //           "title": "Rekta 5"
  //         },
  //         {
  //           "type": "string",
  //           "enum": [
  //             "16b74959-cb05-417e-cecd-08d95afcbf75"
  //           ],
  //           "title": "Rekta 6"
  //         },
  //         {
  //           "type": "string",
  //           "enum": [
  //             "8ea1f7f7-beb8-48c2-cece-08d95afcbf75"
  //           ],
  //           "title": "Rekta 10"
  //         },
  //         {
  //           "type": "string",
  //           "enum": [
  //             "de75e99f-7d19-4315-cecf-08d95afcbf75"
  //           ],
  //           "title": "Lukso 39"
  //         },
  //         {
  //           "type": "string",
  //           "enum": [
  //             "23203251-6334-41ba-ced0-08d95afcbf75"
  //           ],
  //           "title": "Lukso 22"
  //         },
  //         {
  //           "type": "string",
  //           "enum": [
  //             "2874c6de-d908-4518-ced1-08d95afcbf75"
  //           ],
  //           "title": "Lukso 10"
  //         },
  //         {
  //           "type": "string",
  //           "enum": [
  //             "eccee519-cd0a-47c9-ced2-08d95afcbf75"
  //           ],
  //           "title": "Lukso 40"
  //         },
  //         {
  //           "type": "string",
  //           "enum": [
  //             "33b9345a-17fd-4d0b-ced3-08d95afcbf75"
  //           ],
  //           "title": "Lukso 38"
  //         },
  //         {
  //           "type": "string",
  //           "enum": [
  //             "75821847-fcf2-45aa-ced4-08d95afcbf75"
  //           ],
  //           "title": "Lukso 35"
  //         },
  //         {
  //           "type": "string",
  //           "enum": [
  //             "bd9896bd-8766-4759-ced5-08d95afcbf75"
  //           ],
  //           "title": "Lukso 24"
  //         },
  //         {
  //           "type": "string",
  //           "enum": [
  //             "cf902f84-d479-41fb-ced6-08d95afcbf75"
  //           ],
  //           "title": "Sola 18"
  //         },
  //         {
  //           "type": "string",
  //           "enum": [
  //             "5381b460-b664-4ea7-ced7-08d95afcbf75"
  //           ],
  //           "title": "Sola 4"
  //         },
  //         {
  //           "type": "string",
  //           "enum": [
  //             "eff4b988-f9af-4f12-ced8-08d95afcbf75"
  //           ],
  //           "title": "Sola 6"
  //         },
  //         {
  //           "type": "string",
  //           "enum": [
  //             "4f7f9b9c-db39-4ef6-ced9-08d95afcbf75"
  //           ],
  //           "title": "Nube 20"
  //         },
  //         {
  //           "type": "string",
  //           "enum": [
  //             "79b11b7c-1923-4715-ceda-08d95afcbf75"
  //           ],
  //           "title": "Nube 22"
  //         },
  //         {
  //           "type": "string",
  //           "enum": [
  //             "ecea472d-ae15-4be2-cedb-08d95afcbf75"
  //           ],
  //           "title": "Nube 35"
  //         },
  //         {
  //           "type": "string",
  //           "enum": [
  //             "21d18213-8520-4383-cedc-08d95afcbf75"
  //           ],
  //           "title": "Nube 33"
  //         },
  //         {
  //           "type": "string",
  //           "enum": [
  //             "ef4cdace-6479-4abd-cedd-08d95afcbf75"
  //           ],
  //           "title": "Nube 45"
  //         },
  //         {
  //           "type": "string",
  //           "enum": [
  //             "80080f15-a046-4e95-cede-08d95afcbf75"
  //           ],
  //           "title": "Nube 24"
  //         },
  //         {
  //           "type": "string",
  //           "enum": [
  //             "04a01105-9484-4cbb-cedf-08d95afcbf75"
  //           ],
  //           "title": "Nube 40"
  //         },
  //         {
  //           "type": "string",
  //           "enum": [
  //             "0abd1c12-42f1-45c0-cee0-08d95afcbf75"
  //           ],
  //           "title": "Nube 3"
  //         },
  //         {
  //           "type": "string",
  //           "enum": [
  //             "37d562e9-86b5-4959-cee1-08d95afcbf75"
  //           ],
  //           "title": "Nube 4"
  //         },
  //         {
  //           "type": "string",
  //           "enum": [
  //             "8b8148b0-b7ba-482a-cee2-08d95afcbf75"
  //           ],
  //           "title": "Nube 5"
  //         },
  //         {
  //           "type": "string",
  //           "enum": [
  //             "3ffe7d4d-489a-4845-cee3-08d95afcbf75"
  //           ],
  //           "title": "Nube 6"
  //         },
  //         {
  //           "type": "string",
  //           "enum": [
  //             "cee0abf9-120c-479b-cee4-08d95afcbf75"
  //           ],
  //           "title": "Velvetmat 22"
  //         },
  //         {
  //           "type": "string",
  //           "enum": [
  //             "abd46f2a-9f8e-4dc0-cee5-08d95afcbf75"
  //           ],
  //           "title": "Velvetmat 24"
  //         },
  //         {
  //           "type": "string",
  //           "enum": [
  //             "c8b40bac-3f12-47a7-cee6-08d95afcbf75"
  //           ],
  //           "title": "Velvetmat 25"
  //         },
  //         {
  //           "type": "string",
  //           "enum": [
  //             "2a326040-d54a-4ccf-cee7-08d95afcbf75"
  //           ],
  //           "title": "Velvetmat 38"
  //         },
  //         {
  //           "type": "string",
  //           "enum": [
  //             "aeeadc36-c557-4e1a-cee8-08d95afcbf75"
  //           ],
  //           "title": "Velvetmat 40"
  //         },
  //         {
  //           "type": "string",
  //           "enum": [
  //             "40174cce-0b54-46fe-cee9-08d95afcbf75"
  //           ],
  //           "title": "Velvetmat 4"
  //         },
  //         {
  //           "type": "string",
  //           "enum": [
  //             "87f550f5-fe81-4c47-ceea-08d95afcbf75"
  //           ],
  //           "title": "Velvetmat 6"
  //         },
  //         {
  //           "type": "string",
  //           "enum": [
  //             "fad915e6-a960-4c08-ceeb-08d95afcbf75"
  //           ],
  //           "title": "Velvetmat 10"
  //         },
  //         {
  //           "type": "string",
  //           "enum": [
  //             "9ee3788e-404c-4ba7-ceec-08d95afcbf75"
  //           ],
  //           "title": "Loco 21"
  //         },
  //         {
  //           "type": "string",
  //           "enum": [
  //             "14379fbf-edb5-4327-ceed-08d95afcbf75"
  //           ],
  //           "title": "Loco 33"
  //         },
  //         {
  //           "type": "string",
  //           "enum": [
  //             "129b2f04-4e16-486f-ceee-08d95afcbf75"
  //           ],
  //           "title": "Loco 35"
  //         },
  //         {
  //           "type": "string",
  //           "enum": [
  //             "2f938925-076f-4864-ceef-08d95afcbf75"
  //           ],
  //           "title": "Loco 45"
  //         },
  //         {
  //           "type": "string",
  //           "enum": [
  //             "1508cc9a-b747-4e24-cef0-08d95afcbf75"
  //           ],
  //           "title": "Loco 25"
  //         },
  //         {
  //           "type": "string",
  //           "enum": [
  //             "79fb0293-5a86-4403-cef1-08d95afcbf75"
  //           ],
  //           "title": "Loco 40"
  //         },
  //         {
  //           "type": "string",
  //           "enum": [
  //             "21acbaf6-ea6f-4cba-cef2-08d95afcbf75"
  //           ],
  //           "title": "Loco 4"
  //         },
  //         {
  //           "type": "string",
  //           "enum": [
  //             "429453bc-03e4-48c5-cef3-08d95afcbf75"
  //           ],
  //           "title": "Loco 5"
  //         },
  //         {
  //           "type": "string",
  //           "enum": [
  //             "9fe2a61d-90c5-48c9-cef4-08d95afcbf75"
  //           ],
  //           "title": "Loco 10"
  //         },
  //         {
  //           "type": "string",
  //           "enum": [
  //             "d0c429e3-d9fe-4ca4-cef5-08d95afcbf75"
  //           ],
  //           "title": "Sombra 18"
  //         },
  //         {
  //           "type": "string",
  //           "enum": [
  //             "beba473c-239a-48cb-cef6-08d95afcbf75"
  //           ],
  //           "title": "Sombra 4"
  //         },
  //         {
  //           "type": "string",
  //           "enum": [
  //             "61f0d3c2-6496-4ff8-cef7-08d95afcbf75"
  //           ],
  //           "title": "Sombra 10"
  //         },
  //         {
  //           "type": "string",
  //           "enum": [
  //             "b0d2c6ae-9fbc-433e-cef8-08d95afcbf75"
  //           ],
  //           "title": "Rike 18"
  //         },
  //         {
  //           "type": "string",
  //           "enum": [
  //             "e5737154-5387-41f3-cef9-08d95afcbf75"
  //           ],
  //           "title": "Rike 4"
  //         },
  //         {
  //           "type": "string",
  //           "enum": [
  //             "a8115b63-83bb-46e2-cefa-08d95afcbf75"
  //           ],
  //           "title": "Rike 40"
  //         },
  //         {
  //           "type": "string",
  //           "enum": [
  //             "0726c4ce-ab2c-486f-cefb-08d95afcbf75"
  //           ],
  //           "title": "Rike 5"
  //         },
  //         {
  //           "type": "string",
  //           "enum": [
  //             "8849893a-87c1-4a18-cefc-08d95afcbf75"
  //           ],
  //           "title": "Rike 6"
  //         },
  //         {
  //           "type": "string",
  //           "enum": [
  //             "be932481-669d-4632-cefd-08d95afcbf75"
  //           ],
  //           "title": "Borneo 35"
  //         },
  //         {
  //           "type": "string",
  //           "enum": [
  //             "f7265edf-7cde-44fe-cefe-08d95afcbf75"
  //           ],
  //           "title": "Borneo 38"
  //         },
  //         {
  //           "type": "string",
  //           "enum": [
  //             "d71b71b8-8a0d-4bf8-ceff-08d95afcbf75"
  //           ],
  //           "title": "Borneo 40"
  //         },
  //         {
  //           "type": "string",
  //           "enum": [
  //             "b1a64781-fbd1-47ec-cf00-08d95afcbf75"
  //           ],
  //           "title": "Borneo 4"
  //         },
  //         {
  //           "type": "string",
  //           "enum": [
  //             "b376283b-b615-451a-cf01-08d95afcbf75"
  //           ],
  //           "title": "Monte 18"
  //         },
  //         {
  //           "type": "string",
  //           "enum": [
  //             "c830e81b-f99d-4d79-cf02-08d95afcbf75"
  //           ],
  //           "title": "Monte 20"
  //         },
  //         {
  //           "type": "string",
  //           "enum": [
  //             "0227822c-d9cb-4e84-cf03-08d95afcbf75"
  //           ],
  //           "title": "Monte 21"
  //         },
  //         {
  //           "type": "string",
  //           "enum": [
  //             "0c1bfeac-745e-44dc-cf04-08d95afcbf75"
  //           ],
  //           "title": "Monte 6"
  //         },
  //         {
  //           "type": "string",
  //           "enum": [
  //             "d090f8d4-c708-4fce-cf05-08d95afcbf75"
  //           ],
  //           "title": "Monte 4"
  //         },
  //         {
  //           "type": "string",
  //           "enum": [
  //             "856bef52-b48a-4e7b-cf06-08d95afcbf75"
  //           ],
  //           "title": "Marte 130"
  //         },
  //         {
  //           "type": "string",
  //           "enum": [
  //             "e0c5f54f-43a1-4711-cf07-08d95afcbf75"
  //           ],
  //           "title": "Marte 10"
  //         },
  //         {
  //           "type": "string",
  //           "enum": [
  //             "4e0b4eff-2cec-4ddf-cf08-08d95afcbf75"
  //           ],
  //           "title": "Marte 20"
  //         },
  //         {
  //           "type": "string",
  //           "enum": [
  //             "b4463fa7-22da-4d1e-cf09-08d95afcbf75"
  //           ],
  //           "title": "Marte 1"
  //         },
  //         {
  //           "type": "string",
  //           "enum": [
  //             "938cbf19-7aef-44fd-cf0a-08d95afcbf75"
  //           ],
  //           "title": "Softis 9"
  //         },
  //         {
  //           "type": "string",
  //           "enum": [
  //             "8abb5a7d-ba2b-49cf-cf0b-08d95afcbf75"
  //           ],
  //           "title": "Softis 10"
  //         },
  //         {
  //           "type": "string",
  //           "enum": [
  //             "f88403c3-384a-4500-cf0c-08d95afcbf75"
  //           ],
  //           "title": "Softis 11"
  //         },
  //         {
  //           "type": "string",
  //           "enum": [
  //             "966529f3-e753-4570-cf0d-08d95afcbf75"
  //           ],
  //           "title": "Softis 15"
  //         },
  //         {
  //           "type": "string",
  //           "enum": [
  //             "6374f471-0903-4e93-cf0e-08d95afcbf75"
  //           ],
  //           "title": "Softis 16"
  //         },
  //         {
  //           "type": "string",
  //           "enum": [
  //             "1750735d-986c-42b8-cf0f-08d95afcbf75"
  //           ],
  //           "title": "Softis 17"
  //         },
  //         {
  //           "type": "string",
  //           "enum": [
  //             "d366822e-e4b3-48c6-cf10-08d95afcbf75"
  //           ],
  //           "title": "Softis 18"
  //         },
  //         {
  //           "type": "string",
  //           "enum": [
  //             "b98fa66b-dab2-4cb2-cf11-08d95afcbf75"
  //           ],
  //           "title": "Softis 29"
  //         },
  //         {
  //           "type": "string",
  //           "enum": [
  //             "9dd23a31-1046-4fc7-cf12-08d95afcbf75"
  //           ],
  //           "title": "Softis 33"
  //         },
  //         {
  //           "type": "string",
  //           "enum": [
  //             "e5c4789f-1bc0-4c7f-cf13-08d95afcbf75"
  //           ],
  //           "title": "Softis 66"
  //         },
  //         {
  //           "type": "string",
  //           "enum": [
  //             "aec00cd8-8d19-4aad-cf14-08d95afcbf75"
  //           ],
  //           "title": "Softis 908"
  //         },
  //         {
  //           "type": "string",
  //           "enum": [
  //             "90b8a70c-72cc-4708-cf15-08d95afcbf75"
  //           ],
  //           "title": "Softis 985"
  //         },
  //         {
  //           "type": "string",
  //           "enum": [
  //             "00857a0f-4c54-4868-cf16-08d95afcbf75"
  //           ],
  //           "title": "Savoi 1"
  //         },
  //         {
  //           "type": "string",
  //           "enum": [
  //             "8621ce8c-2ddc-4ecd-cf17-08d95afcbf75"
  //           ],
  //           "title": "Savoi 7"
  //         },
  //         {
  //           "type": "string",
  //           "enum": [
  //             "c06896b5-4a12-4c48-cf18-08d95afcbf75"
  //           ],
  //           "title": "Savoi 45"
  //         },
  //         {
  //           "type": "string",
  //           "enum": [
  //             "47c78e5a-e471-4fd0-cf19-08d95afcbf75"
  //           ],
  //           "title": "Savoi 24"
  //         },
  //         {
  //           "type": "string",
  //           "enum": [
  //             "b2d0019e-a4ab-4304-cf1a-08d95afcbf75"
  //           ],
  //           "title": "Savoi 100"
  //         },
  //         {
  //           "type": "string",
  //           "enum": [
  //             "7ea8f1cd-c925-473b-cf1b-08d95afcbf75"
  //           ],
  //           "title": "Savoi 38"
  //         },
  //         {
  //           "type": "string",
  //           "enum": [
  //             "d0af3f0d-7376-4cfe-cf1c-08d95afcbf75"
  //           ],
  //           "title": "Savoi 40"
  //         },
  //         {
  //           "type": "string",
  //           "enum": [
  //             "7ebfc6b4-4d7c-48d3-cf1d-08d95afcbf75"
  //           ],
  //           "title": "Savoi 6"
  //         },
  //         {
  //           "type": "string",
  //           "enum": [
  //             "19f0f916-99e2-4714-cf1e-08d95afcbf75"
  //           ],
  //           "title": "Savoi 10"
  //         },
  //         {
  //           "type": "string",
  //           "enum": [
  //             "35097ff8-70e8-4173-cf1f-08d95afcbf75"
  //           ],
  //           "title": "Leve 18"
  //         },
  //         {
  //           "type": "string",
  //           "enum": [
  //             "d740c956-9df3-48f0-cf20-08d95afcbf75"
  //           ],
  //           "title": "Leve 20"
  //         },
  //         {
  //           "type": "string",
  //           "enum": [
  //             "f7431041-cc7f-4c9f-cf21-08d95afcbf75"
  //           ],
  //           "title": "Leve 21"
  //         },
  //         {
  //           "type": "string",
  //           "enum": [
  //             "c59503ff-902d-4669-cf22-08d95afcbf75"
  //           ],
  //           "title": "Leve 22"
  //         },
  //         {
  //           "type": "string",
  //           "enum": [
  //             "2223b4fe-bfc4-48cc-cf23-08d95afcbf75"
  //           ],
  //           "title": "Leve 25"
  //         },
  //         {
  //           "type": "string",
  //           "enum": [
  //             "917f06ca-1b3d-4e52-cf24-08d95afcbf75"
  //           ],
  //           "title": "Leve 38"
  //         },
  //         {
  //           "type": "string",
  //           "enum": [
  //             "f1c1300f-6354-4a99-cf25-08d95afcbf75"
  //           ],
  //           "title": "Leve 4"
  //         },
  //         {
  //           "type": "string",
  //           "enum": [
  //             "26fdb489-1bf8-4826-cf26-08d95afcbf75"
  //           ],
  //           "title": "Leve 5"
  //         },
  //         {
  //           "type": "string",
  //           "enum": [
  //             "70ccc8ea-1301-48c7-cf27-08d95afcbf75"
  //           ],
  //           "title": "Leve 6"
  //         },
  //         {
  //           "type": "string",
  //           "enum": [
  //             "b95e366d-ab61-4653-cf28-08d95afcbf75"
  //           ],
  //           "title": "Rosa 14"
  //         },
  //         {
  //           "type": "string",
  //           "enum": [
  //             "c491c3ff-85b4-4a10-cf29-08d95afcbf75"
  //           ],
  //           "title": "Selva 32"
  //         },
  //         {
  //           "type": "string",
  //           "enum": [
  //             "01c99793-60e5-4117-cf2a-08d95afcbf75"
  //           ],
  //           "title": "Selva 37"
  //         },
  //         {
  //           "type": "string",
  //           "enum": [
  //             "0bd31c43-258d-45d4-cf2b-08d95afcbf75"
  //           ],
  //           "title": "Rosa 16"
  //         },
  //         {
  //           "type": "string",
  //           "enum": [
  //             "fb3d4a3a-5773-40fc-cf2c-08d95afcbf75"
  //           ],
  //           "title": "Palm 80"
  //         },
  //         {
  //           "type": "string",
  //           "enum": [
  //             "11551341-e3d6-4451-cf2d-08d95afcbf75"
  //           ],
  //           "title": "Marble 2-76"
  //         },
  //         {
  //           "type": "string",
  //           "enum": [
  //             "28797b1f-25b4-422a-cf2e-08d95afcbf75"
  //           ],
  //           "title": "Triangles 67"
  //         },
  //         {
  //           "type": "string",
  //           "enum": [
  //             "3a094df6-47b2-4cb9-cf2f-08d95afcbf75"
  //           ],
  //           "title": "Triangles 75"
  //         },
  //         {
  //           "type": "string",
  //           "enum": [
  //             "7fa9d666-e76d-4a1d-cf30-08d95afcbf75"
  //           ],
  //           "title": "Twist 53"
  //         },
  //         {
  //           "type": "string",
  //           "enum": [
  //             "2ddd3efb-fa8e-4f76-cf31-08d95afcbf75"
  //           ],
  //           "title": "Twist 60"
  //         },
  //         {
  //           "type": "string",
  //           "enum": [
  //             "223ef1fc-f251-49f2-c768-08d96d513e25"
  //           ],
  //           "title": "Cover 83 Pasy 2"
  //         },
  //         {
  //           "type": "string",
  //           "enum": [
  //             "68e43cc2-d883-4b0b-c769-08d96d513e25"
  //           ],
  //           "title": "Jasmine 85 Pasy 2"
  //         },
  //         {
  //           "type": "string",
  //           "enum": [
  //             "04066291-96d1-4528-c76a-08d96d513e25"
  //           ],
  //           "title": "Jasmine 90 Pasy 2"
  //         },
  //         {
  //           "type": "string",
  //           "enum": [
  //             "532123b2-a585-4b33-c76b-08d96d513e25"
  //           ],
  //           "title": "Monolith 29 Pasy 2"
  //         },
  //         {
  //           "type": "string",
  //           "enum": [
  //             "898eb853-f1a0-4c69-c76c-08d96d513e25"
  //           ],
  //           "title": "Monolith 09 Pasy 2"
  //         },
  //         {
  //           "type": "string",
  //           "enum": [
  //             "e3a5dc17-546d-42b7-c76d-08d96d513e25"
  //           ],
  //           "title": "Monolith 37 Pasy 2"
  //         },
  //         {
  //           "type": "string",
  //           "enum": [
  //             "fedbe6ed-9568-4272-c76e-08d96d513e25"
  //           ],
  //           "title": "Monolith 77 Pasy 2"
  //         },
  //         {
  //           "type": "string",
  //           "enum": [
  //             "dd5252ac-b33b-4b95-c76f-08d96d513e25"
  //           ],
  //           "title": "Ontario 100 Pasy 2"
  //         },
  //         {
  //           "type": "string",
  //           "enum": [
  //             "c6c404af-a9da-4b17-c770-08d96d513e25"
  //           ],
  //           "title": "Orinoco 21 Pasy 2"
  //         },
  //         {
  //           "type": "string",
  //           "enum": [
  //             "e33fd3d1-3bc3-4cf2-c771-08d96d513e25"
  //           ],
  //           "title": "Sawana 05 Pasy 2"
  //         },
  //         {
  //           "type": "string",
  //           "enum": [
  //             "fd3cfce7-eb7e-4dfc-c772-08d96d513e25"
  //           ],
  //           "title": "Sawana 14 Pasy 2"
  //         },
  //         {
  //           "type": "string",
  //           "enum": [
  //             "9419bd1c-9298-4106-c773-08d96d513e25"
  //           ],
  //           "title": "Sawana 21 Pasy 2"
  //         },
  //         {
  //           "type": "string",
  //           "enum": [
  //             "00bbc3ce-c40f-469c-c774-08d96d513e25"
  //           ],
  //           "title": "Soft 11 Pasy 2"
  //         },
  //         {
  //           "type": "string",
  //           "enum": [
  //             "ff1d731e-86f5-479a-c775-08d96d513e25"
  //           ],
  //           "title": "Soft 17 Pasy 2"
  //         },
  //         {
  //           "type": "string",
  //           "enum": [
  //             "2cc9a85e-cb25-4e02-c776-08d96d513e25"
  //           ],
  //           "title": "Matt Velvet 68 Pasy 2"
  //         },
  //         {
  //           "type": "string",
  //           "enum": [
  //             "5d03d81e-e5df-4eb1-c777-08d96d513e25"
  //           ],
  //           "title": "Dora 21 Pasy 2"
  //         },
  //         {
  //           "type": "string",
  //           "enum": [
  //             "27a6a2e2-8724-425e-c778-08d96d513e25"
  //           ],
  //           "title": "Dora 22 Pasy 2"
  //         },
  //         {
  //           "type": "string",
  //           "enum": [
  //             "ec3943b7-3db6-42e5-c779-08d96d513e25"
  //           ],
  //           "title": "Soro 61 Pasy 2"
  //         },
  //         {
  //           "type": "string",
  //           "enum": [
  //             "a9266ea9-5759-4691-c77a-08d96d513e25"
  //           ],
  //           "title": "Soro 83 Pasy 2"
  //         },
  //         {
  //           "type": "string",
  //           "enum": [
  //             "8d9a73c0-99bf-4644-c77b-08d96d513e25"
  //           ],
  //           "title": "Gusto 88 Pasy 2"
  //         },
  //         {
  //           "type": "string",
  //           "enum": [
  //             "7be09115-77cb-4155-c77c-08d96d513e25"
  //           ],
  //           "title": "Matt Velvet 75 Pasy 2"
  //         },
  //         {
  //           "type": "string",
  //           "enum": [
  //             "879bfaa9-f70f-49e7-c77d-08d96d513e25"
  //           ],
  //           "title": "Monolith 83 Pasy 2"
  //         },
  //         {
  //           "type": "string",
  //           "enum": [
  //             "37b4e780-5a7b-45a9-c77e-08d96d513e25"
  //           ],
  //           "title": "Monolith 48 Pasy 2"
  //         },
  //         {
  //           "type": "string",
  //           "enum": [
  //             "0f602c5c-c01a-42bd-c77f-08d96d513e25"
  //           ],
  //           "title": "Inari 96 Pasy 2"
  //         },
  //         {
  //           "type": "string",
  //           "enum": [
  //             "94f42be9-f4b3-4803-c780-08d96d513e25"
  //           ],
  //           "title": "Inari 100 Pasy 2"
  //         },
  //         {
  //           "type": "string",
  //           "enum": [
  //             "55bd8bed-6416-4424-c781-08d96d513e25"
  //           ],
  //           "title": "Paros 02 Pasy 2"
  //         },
  //         {
  //           "type": "string",
  //           "enum": [
  //             "24e44bc0-facf-43c1-c782-08d96d513e25"
  //           ],
  //           "title": "Paros 05 Pasy 2"
  //         },
  //         {
  //           "type": "string",
  //           "enum": [
  //             "b62b03f6-3a0b-4d3a-c783-08d96d513e25"
  //           ],
  //           "title": "Kronos 09  Pasy 2"
  //         },
  //         {
  //           "type": "string",
  //           "enum": [
  //             "6e1aa8d1-7ed0-47d0-c784-08d96d513e25"
  //           ],
  //           "title": "Kronos 19  Pasy 2"
  //         },
  //         {
  //           "type": "string",
  //           "enum": [
  //             "49d1422c-0877-4a9a-c785-08d96d513e25"
  //           ],
  //           "title": "Kronos 29  Pasy 2 "
  //         },
  //         {
  //           "type": "string",
  //           "enum": [
  //             "65f7c044-acce-4b7b-c786-08d96d513e25"
  //           ],
  //           "title": "Monolith 63 Pasy 2 "
  //         },
  //         {
  //           "type": "string",
  //           "enum": [
  //             "c2f43d4e-caba-4252-c787-08d96d513e25"
  //           ],
  //           "title": "Matt Velvet 63 Pasy 2"
  //         },
  //         {
  //           "type": "string",
  //           "enum": [
  //             "807b432c-a6c9-415a-c788-08d96d513e25"
  //           ],
  //           "title": "Matt Velvet 99 Pasy 2"
  //         },
  //         {
  //           "type": "string",
  //           "enum": [
  //             "e33bec02-d190-471c-c789-08d96d513e25"
  //           ],
  //           "title": "Monolith 97 Pasy 2"
  //         },
  //         {
  //           "type": "string",
  //           "enum": [
  //             "1cc3ffd8-6ac2-490a-c78a-08d96d513e25"
  //           ],
  //           "title": "Monolith 85 Pasy 2"
  //         },
  //         {
  //           "type": "string",
  //           "enum": [
  //             "c42e26a5-da26-4c61-c78b-08d96d513e25"
  //           ],
  //           "title": "Monolith 84 Pasy 2"
  //         },
  //         {
  //           "type": "string",
  //           "enum": [
  //             "e1d2923c-49ef-4e3b-1ce6-08d98580730e"
  //           ],
  //           "title": "Aloevera"
  //         },
  //         {
  //           "type": "string",
  //           "enum": [
  //             "d721fba8-25b5-4cc5-1ce7-08d98580730e"
  //           ],
  //           "title": "Medicott Silver"
  //         },
  //         {
  //           "type": "string",
  //           "enum": [
  //             "22fc2d3e-c7cc-48a6-1ce8-08d98580730e"
  //           ],
  //           "title": "Silk"
  //         },
  //         {
  //           "type": "string",
  //           "enum": [
  //             "07a01380-cae5-464a-1ce9-08d98580730e"
  //           ],
  //           "title": "Cashmere+Velvet czarny"
  //         },
  //         {
  //           "type": "string",
  //           "enum": [
  //             "81f0ecd9-58de-4415-1cea-08d98580730e"
  //           ],
  //           "title": "Cashmere"
  //         },
  //         {
  //           "type": "string",
  //           "enum": [
  //             "6cee3314-9f3a-4c8c-1ceb-08d98580730e"
  //           ],
  //           "title": "Velvet Czarny"
  //         },
  //         {
  //           "type": "string",
  //           "enum": [
  //             "333a5611-4aac-4d38-1cec-08d98580730e"
  //           ],
  //           "title": "Malaga 01"
  //         },
  //         {
  //           "type": "string",
  //           "enum": [
  //             "a6b61559-370a-49f3-1ced-08d98580730e"
  //           ],
  //           "title": "Malaga 01 Pasy 2"
  //         }
  //       ]
  //     },
  //     "3cb42bca-84d9-423f-e4ee-08d907680a85": {
  //       "title": "Shape",
  //       "type": "string",
  //       "anyOf": [
  //         {
  //           "type": "string",
  //           "enum": [
  //             "eeeee216-07ec-4ab4-5dd3-08d907680c53"
  //           ],
  //           "title": "L"
  //         },
  //         {
  //           "type": "string",
  //           "enum": [
  //             "23bb49c3-5b1d-4a5d-5dd4-08d907680c53"
  //           ],
  //           "title": "U"
  //         }
  //       ]
  //     },
  //     "a0a08692-3bfc-4b98-e4ef-08d907680a85": {
  //       "title": "Orientation",
  //       "type": "string",
  //       "anyOf": [
  //         {
  //           "type": "string",
  //           "enum": [
  //             "2f64d839-381b-4a0c-5dd5-08d907680c53"
  //           ],
  //           "title": "Left"
  //         },
  //         {
  //           "type": "string",
  //           "enum": [
  //             "98b4d01d-6cc3-4f59-5dd6-08d907680c53"
  //           ],
  //           "title": "Reversible"
  //         },
  //         {
  //           "type": "string",
  //           "enum": [
  //             "bac0fb87-452e-4348-5dd7-08d907680c53"
  //           ],
  //           "title": "Right"
  //         },
  //         {
  //           "type": "string",
  //           "enum": [
  //             "7929f45e-2f64-43ac-5dd8-08d907680c53"
  //           ],
  //           "title": "Symmetrical"
  //         }
  //       ]
  //     },
  //     "3287b56b-c4cd-4ff9-e4f0-08d907680a85": {
  //       "title": "Upholstery Material",
  //       "type": "string",
  //       "anyOf": [
  //         {
  //           "type": "string",
  //           "enum": [
  //             "c6c39d93-5ca6-4f81-5dd9-08d907680c53"
  //           ],
  //           "title": "Polyester"
  //         }
  //       ]
  //     },
  //     "0b907e7d-947d-4d62-e4f1-08d907680a85": {
  //       "title": "Fill Material",
  //       "type": "string",
  //       "anyOf": [
  //         {
  //           "type": "string",
  //           "enum": [
  //             "374e031c-0f95-4cac-5dda-08d907680c53"
  //           ],
  //           "title": "Bonnell"
  //         },
  //         {
  //           "type": "string",
  //           "enum": [
  //             "898d7a94-410f-41cc-5ddb-08d907680c53"
  //           ],
  //           "title": "Coconut Mat"
  //         },
  //         {
  //           "type": "string",
  //           "enum": [
  //             "0ec65479-ddca-4a07-5ddc-08d907680c53"
  //           ],
  //           "title": "Felt"
  //         },
  //         {
  //           "type": "string",
  //           "enum": [
  //             "5a026368-c2ec-4465-5ddd-08d907680c53"
  //           ],
  //           "title": "Foam"
  //         },
  //         {
  //           "type": "string",
  //           "enum": [
  //             "2707c217-1d1b-479f-5dde-08d907680c53"
  //           ],
  //           "title": "Foam Latex"
  //         },
  //         {
  //           "type": "string",
  //           "enum": [
  //             "fe60ff6b-5331-40c0-5ddf-08d907680c53"
  //           ],
  //           "title": "Foam Pur-Fabric"
  //         },
  //         {
  //           "type": "string",
  //           "enum": [
  //             "59befb2a-d71f-4c2d-5de0-08d907680c53"
  //           ],
  //           "title": "Foam Visco"
  //         },
  //         {
  //           "type": "string",
  //           "enum": [
  //             "73fa32aa-72be-4559-5de1-08d907680c53"
  //           ],
  //           "title": "Foam-High Quality Hr"
  //         },
  //         {
  //           "type": "string",
  //           "enum": [
  //             "db103a1b-9b80-4ed0-5de2-08d907680c53"
  //           ],
  //           "title": "Foam-Hr"
  //         },
  //         {
  //           "type": "string",
  //           "enum": [
  //             "5419d5f8-42b5-4b4d-5de3-08d907680c53"
  //           ],
  //           "title": "Foam-T21"
  //         },
  //         {
  //           "type": "string",
  //           "enum": [
  //             "f6374f45-e0b9-4cd6-5de4-08d907680c53"
  //           ],
  //           "title": "Foam-T25"
  //         },
  //         {
  //           "type": "string",
  //           "enum": [
  //             "d12b1f6f-61ae-48df-5de5-08d907680c53"
  //           ],
  //           "title": "Foam-T2540"
  //         },
  //         {
  //           "type": "string",
  //           "enum": [
  //             "c96d7903-a21c-4823-5de6-08d907680c53"
  //           ],
  //           "title": "Foam-T2541"
  //         },
  //         {
  //           "type": "string",
  //           "enum": [
  //             "6acf4dc7-f897-4956-5de7-08d907680c53"
  //           ],
  //           "title": "Foam-T2542"
  //         },
  //         {
  //           "type": "string",
  //           "enum": [
  //             "22822abe-30bd-4960-5de8-08d907680c53"
  //           ],
  //           "title": "Foam-T2542  Latex"
  //         },
  //         {
  //           "type": "string",
  //           "enum": [
  //             "e3b0e453-30ff-4e96-5de9-08d907680c53"
  //           ],
  //           "title": "Foam-T2543"
  //         },
  //         {
  //           "type": "string",
  //           "enum": [
  //             "62525319-2964-4727-5dea-08d907680c53"
  //           ],
  //           "title": "Foam-T2544"
  //         },
  //         {
  //           "type": "string",
  //           "enum": [
  //             "74e82e0a-4e09-400e-5deb-08d907680c53"
  //           ],
  //           "title": "Foam-T2545"
  //         },
  //         {
  //           "type": "string",
  //           "enum": [
  //             "f1addf26-ee46-4a39-5dec-08d907680c53"
  //           ],
  //           "title": "Foam-T2546"
  //         },
  //         {
  //           "type": "string",
  //           "enum": [
  //             "b84ce2d5-be9f-4fba-5ded-08d907680c53"
  //           ],
  //           "title": "Foam-T2547"
  //         },
  //         {
  //           "type": "string",
  //           "enum": [
  //             "6a4c11a1-19a6-47d0-5dee-08d907680c53"
  //           ],
  //           "title": "Foam-T2548"
  //         },
  //         {
  //           "type": "string",
  //           "enum": [
  //             "4ca34742-275b-4b06-5def-08d907680c53"
  //           ],
  //           "title": "Foam-T30"
  //         },
  //         {
  //           "type": "string",
  //           "enum": [
  //             "b3485a3f-b1d6-4e56-5df0-08d907680c53"
  //           ],
  //           "title": "Foam-T3030"
  //         },
  //         {
  //           "type": "string",
  //           "enum": [
  //             "51d3b574-4766-448b-5df1-08d907680c53"
  //           ],
  //           "title": "Latex"
  //         },
  //         {
  //           "type": "string",
  //           "enum": [
  //             "e0e84bc5-b16e-4249-5df2-08d907680c53"
  //           ],
  //           "title": "Multipocket Spring"
  //         },
  //         {
  //           "type": "string",
  //           "enum": [
  //             "fa72329f-6712-45ea-5df3-08d907680c53"
  //           ],
  //           "title": "Pocket Spring"
  //         },
  //         {
  //           "type": "string",
  //           "enum": [
  //             "5bb3eb83-529b-4406-5df4-08d907680c53"
  //           ],
  //           "title": "Profiled Foam-Hr"
  //         }
  //       ]
  //     },
  //     "699e1291-0b62-410d-e4f2-08d907680a85": {
  //       "title": "Frame Material",
  //       "type": "string",
  //       "anyOf": [
  //         {
  //           "type": "string",
  //           "enum": [
  //             "941b3a5c-89de-4b46-5df5-08d907680c53"
  //           ],
  //           "title": "Metal"
  //         },
  //         {
  //           "type": "string",
  //           "enum": [
  //             "da900a5c-900d-4c0d-5df6-08d907680c53"
  //           ],
  //           "title": "Steel"
  //         },
  //         {
  //           "type": "string",
  //           "enum": [
  //             "7970ba25-cc98-40a3-5df7-08d907680c53"
  //           ],
  //           "title": "Wood"
  //         }
  //       ]
  //     },
  //     "4f8404ec-2fcc-40ae-e4f3-08d907680a85": {
  //       "title": "Frame Wood Type",
  //       "type": "string",
  //       "anyOf": [
  //         {
  //           "type": "string",
  //           "enum": [
  //             "1e5a34d2-5435-4bda-5df8-08d907680c53"
  //           ],
  //           "title": "Pine"
  //         },
  //         {
  //           "type": "string",
  //           "enum": [
  //             "8ecd7e1c-a289-474f-5df9-08d907680c53"
  //           ],
  //           "title": "Steel"
  //         }
  //       ]
  //     },
  //     "5a1fbe3d-6092-48fc-e4f4-08d907680a85": {
  //       "title": "Feet Material",
  //       "type": "string",
  //       "anyOf": [
  //         {
  //           "type": "string",
  //           "enum": [
  //             "a0b6e99c-f6d6-4a4e-5dfa-08d907680c53"
  //           ],
  //           "title": "Metal"
  //         },
  //         {
  //           "type": "string",
  //           "enum": [
  //             "4c5e86f1-7552-41ce-5dfb-08d907680c53"
  //           ],
  //           "title": "Plastics"
  //         },
  //         {
  //           "type": "string",
  //           "enum": [
  //             "056ffa01-7b88-4fc6-5dfc-08d907680c53"
  //           ],
  //           "title": "Steel"
  //         },
  //         {
  //           "type": "string",
  //           "enum": [
  //             "870b2257-fdda-472c-5dfd-08d907680c53"
  //           ],
  //           "title": "Wood"
  //         }
  //       ]
  //     },
  //     "69b825d7-9656-4da9-e4f5-08d907680a85": {
  //       "title": "Unfold Mechanism",
  //       "type": "string",
  //       "anyOf": [
  //         {
  //           "type": "string",
  //           "enum": [
  //             "df13c5f1-b4c2-4db5-5dfe-08d907680c53"
  //           ],
  //           "title": "Dl"
  //         },
  //         {
  //           "type": "string",
  //           "enum": [
  //             "579827d5-16a8-4c6b-5dff-08d907680c53"
  //           ],
  //           "title": "Dolphin"
  //         },
  //         {
  //           "type": "string",
  //           "enum": [
  //             "37e137f6-fe5d-4763-5e00-08d907680c53"
  //           ],
  //           "title": "Electrically Adjustable Frame By Using The Remote Control"
  //         },
  //         {
  //           "type": "string",
  //           "enum": [
  //             "8d741328-287f-4a9a-5e01-08d907680c53"
  //           ],
  //           "title": "Klick-Klack"
  //         },
  //         {
  //           "type": "string",
  //           "enum": [
  //             "ed15914a-266c-4cf1-5e02-08d907680c53"
  //           ],
  //           "title": "On Rolls"
  //         },
  //         {
  //           "type": "string",
  //           "enum": [
  //             "b7bb2e5e-b422-4bdb-5e03-08d907680c53"
  //           ],
  //           "title": "On Wheels"
  //         }
  //       ]
  //     },
  //     "823dcdd3-9762-4302-e4f6-08d907680a85": {
  //       "title": "Arm Type",
  //       "type": "string",
  //       "anyOf": [
  //         {
  //           "type": "string",
  //           "enum": [
  //             "8da83014-750e-4f03-5e04-08d907680c53"
  //           ],
  //           "title": "Armless"
  //         },
  //         {
  //           "type": "string",
  //           "enum": [
  //             "8703a173-9903-4ef8-5e05-08d907680c53"
  //           ],
  //           "title": "Flared Arms"
  //         },
  //         {
  //           "type": "string",
  //           "enum": [
  //             "06f326d5-e9c9-46e8-5e06-08d907680c53"
  //           ],
  //           "title": "Pillow Top Arms"
  //         },
  //         {
  //           "type": "string",
  //           "enum": [
  //             "bbe7846d-19a7-4244-5e07-08d907680c53"
  //           ],
  //           "title": "Recessed Arms"
  //         },
  //         {
  //           "type": "string",
  //           "enum": [
  //             "cb9b83d5-b22d-4c7b-5e08-08d907680c53"
  //           ],
  //           "title": "Round Arms"
  //         },
  //         {
  //           "type": "string",
  //           "enum": [
  //             "8a89b50a-9475-4c12-5e09-08d907680c53"
  //           ],
  //           "title": "Scroll Arms"
  //         },
  //         {
  //           "type": "string",
  //           "enum": [
  //             "b3e06e72-1d77-4b4d-5e0a-08d907680c53"
  //           ],
  //           "title": "Square Arms"
  //         }
  //       ]
  //     },
  //     "2586012a-c112-4dc9-e4f7-08d907680a85": {
  //       "title": "Back Type",
  //       "type": "string",
  //       "anyOf": [
  //         {
  //           "type": "string",
  //           "enum": [
  //             "7d22e93d-9af2-4491-5e0b-08d907680c53"
  //           ],
  //           "title": "Biscuit Back"
  //         },
  //         {
  //           "type": "string",
  //           "enum": [
  //             "12255afb-3389-4318-5e0c-08d907680c53"
  //           ],
  //           "title": "Button Back"
  //         },
  //         {
  //           "type": "string",
  //           "enum": [
  //             "06d73e81-d9b2-4a88-5e0d-08d907680c53"
  //           ],
  //           "title": "Camel Back"
  //         },
  //         {
  //           "type": "string",
  //           "enum": [
  //             "675e60e2-c669-45ed-5e0e-08d907680c53"
  //           ],
  //           "title": "Cushion Back"
  //         },
  //         {
  //           "type": "string",
  //           "enum": [
  //             "94481e67-01f0-46ad-5e0f-08d907680c53"
  //           ],
  //           "title": "Fixed Back"
  //         },
  //         {
  //           "type": "string",
  //           "enum": [
  //             "176bd007-ca9f-4886-5e10-08d907680c53"
  //           ],
  //           "title": "Loose Back"
  //         },
  //         {
  //           "type": "string",
  //           "enum": [
  //             "0b81f1c1-4f88-426b-5e11-08d907680c53"
  //           ],
  //           "title": "Scatter Back"
  //         }
  //       ]
  //     },
  //     "ecbf64d9-2340-4464-e4f8-08d907680a85": {
  //       "title": "Style",
  //       "type": "string",
  //       "anyOf": [
  //         {
  //           "type": "string",
  //           "enum": [
  //             "df70874e-c63f-419b-5e12-08d907680c53"
  //           ],
  //           "title": "Modern"
  //         },
  //         {
  //           "type": "string",
  //           "enum": [
  //             "1e6102c6-e837-45ad-5e13-08d907680c53"
  //           ],
  //           "title": "Scandinavian"
  //         },
  //         {
  //           "type": "string",
  //           "enum": [
  //             "ec8c8221-5ebc-4f30-5e14-08d907680c53"
  //           ],
  //           "title": "Traditional"
  //         }
  //       ]
  //     },
  //     "a92b072d-149c-4e09-e4ed-08d907680a85": {
  //       "title": "Country",
  //       "type": "string",
  //       "anyOf": [
  //         {
  //           "type": "string",
  //           "enum": [
  //             "e51b75df-249d-4a66-5dd2-08d907680c53"
  //           ],
  //           "title": "Poland"
  //         }
  //       ]
  //     },
  //     "3c1a2fba-ca0e-49da-e4f9-08d907680a85": {
  //       "title": "Approved Use",
  //       "type": "string",
  //       "anyOf": [
  //         {
  //           "type": "string",
  //           "enum": [
  //             "bb554b4e-7a4d-447f-5e15-08d907680c53"
  //           ],
  //           "title": "Residential Use"
  //         }
  //       ]
  //     },
  //     "50113561-2f26-4695-e4fa-08d907680a85": {
  //       "title": "Level Of Assembly",
  //       "type": "string",
  //       "anyOf": [
  //         {
  //           "type": "string",
  //           "enum": [
  //             "51ba8bad-e2a8-4d12-5e16-08d907680c53"
  //           ],
  //           "title": "Partially Assembled"
  //         }
  //       ]
  //     },
  //     "560e339a-1ef0-401e-e4fb-08d907680a85": {
  //       "title": "Warranty",
  //       "type": "string",
  //       "anyOf": [
  //         {
  //           "type": "string",
  //           "enum": [
  //             "9382ad46-b1f4-4855-5e17-08d907680c53"
  //           ],
  //           "title": "2 Years"
  //         }
  //       ]
  //     }
  //   },
  //   "type": "object",
  //   "properties": {
  //     "length": {
  //       "type": "number",
  //       "title": "Length [cm]"
  //     },
  //     "width": {
  //       "type": "number",
  //       "title": "Width [cm]"
  //     },
  //     "height": {
  //       "type": "number",
  //       "title": "Height [cm]"
  //     },
  //     "depth": {
  //       "type": "number",
  //       "title": "Depth [cm]"
  //     },
  //     "shape": {
  //       "$ref": "#/definitions/3cb42bca-84d9-423f-e4ee-08d907680a85",
  //       "title": "Shape"
  //     },
  //     "orientation": {
  //       "$ref": "#/definitions/a0a08692-3bfc-4b98-e4ef-08d907680a85",
  //       "title": "Orientation"
  //     },
  //     "style": {
  //       "$ref": "#/definitions/ecbf64d9-2340-4464-e4f8-08d907680a85",
  //       "title": "Style"
  //     },
  //     "primaryColor": {
  //       "$ref": "#/definitions/a04b3368-fa25-4b4a-e4eb-08d907680a85",
  //       "title": "Primary Color"
  //     },
  //     "secondaryColor": {
  //       "$ref": "#/definitions/a04b3368-fa25-4b4a-e4eb-08d907680a85",
  //       "title": "Secondary Color"
  //     },
  //     "primaryFabrics": {
  //       "$ref": "#/definitions/be472892-6902-4736-e4ec-08d907680a85",
  //       "title": "Primary Fabrics"
  //     },
  //     "secondaryFabrics": {
  //       "$ref": "#/definitions/be472892-6902-4736-e4ec-08d907680a85",
  //       "title": "Secondary Fabrics"
  //     },
  //     "upholsteryMaterial": {
  //       "type": "array",
  //       "uniqueItems": true,
  //       "title": "Upholstery Material",
  //       "items": {
  //         "$ref": "#/definitions/3287b56b-c4cd-4ff9-e4f0-08d907680a85"
  //       }
  //     },
  //     "seatAreaWidth": {
  //       "type": "number",
  //       "title": "Seat Area Width [cm]"
  //     },
  //     "seatAreaHeight": {
  //       "type": "number",
  //       "title": "Seat Area Height [cm]"
  //     },
  //     "seatAreaDepth": {
  //       "type": "number",
  //       "title": "Seat Area Depth [cm]"
  //     },
  //     "seatCapacity": {
  //       "type": "number",
  //       "title": "Seat Capacity"
  //     },
  //     "seatFillMaterial": {
  //       "type": "array",
  //       "uniqueItems": true,
  //       "title": "Seat Fill Material",
  //       "items": {
  //         "$ref": "#/definitions/0b907e7d-947d-4d62-e4f1-08d907680a85"
  //       }
  //     },
  //     "weightCapacity": {
  //       "type": "number",
  //       "title": "Weight Capacity [kg]"
  //     },
  //     "sleepFunction": {
  //       "type": "boolean",
  //       "title": "Sleep Function"
  //     },
  //     "sleepAreaWidth": {
  //       "type": "number",
  //       "title": "Sleep Area Width [cm]"
  //     },
  //     "sleepAreaDepth": {
  //       "type": "number",
  //       "title": "Sleep Area Depth [cm]"
  //     },
  //     "unfoldMechanism": {
  //       "$ref": "#/definitions/69b825d7-9656-4da9-e4f5-08d907680a85",
  //       "title": "Unfold Mechanism"
  //     },
  //     "cushionsNumber": {
  //       "type": "number",
  //       "title": "Number of Cushions"
  //     },
  //     "cushionsRemovable": {
  //       "type": "boolean",
  //       "title": "Cushions Removable"
  //     },
  //     "cushionsUpholsteryMaterial": {
  //       "type": "array",
  //       "uniqueItems": true,
  //       "title": "Cushions Upholstery Material",
  //       "items": {
  //         "$ref": "#/definitions/3287b56b-c4cd-4ff9-e4f0-08d907680a85"
  //       }
  //     },
  //     "cushionsFillMaterial": {
  //       "type": "array",
  //       "uniqueItems": true,
  //       "title": "Cushions Fill Material",
  //       "items": {
  //         "$ref": "#/definitions/0b907e7d-947d-4d62-e4f1-08d907680a85"
  //       }
  //     },
  //     "tossPillowsIncluded": {
  //       "type": "boolean",
  //       "title": "Toss Pillows Included"
  //     },
  //     "legWidth": {
  //       "type": "number",
  //       "title": "Leg Width [cm]"
  //     },
  //     "legHeight": {
  //       "type": "number",
  //       "title": "Leg Height [cm]"
  //     },
  //     "legDepth": {
  //       "type": "number",
  //       "title": "Leg Depth [cm]"
  //     },
  //     "legColor": {
  //       "$ref": "#/definitions/a04b3368-fa25-4b4a-e4eb-08d907680a85",
  //       "title": "Leg Color"
  //     },
  //     "legMaterial": {
  //       "type": "array",
  //       "uniqueItems": true,
  //       "title": "Leg Material",
  //       "items": {
  //         "$ref": "#/definitions/5a1fbe3d-6092-48fc-e4f4-08d907680a85"
  //       }
  //     },
  //     "armType": {
  //       "type": "array",
  //       "uniqueItems": true,
  //       "title": "Arm Type",
  //       "items": {
  //         "$ref": "#/definitions/823dcdd3-9762-4302-e4f6-08d907680a85"
  //       }
  //     },
  //     "backType": {
  //       "type": "array",
  //       "uniqueItems": true,
  //       "title": "Back Type",
  //       "items": {
  //         "$ref": "#/definitions/2586012a-c112-4dc9-e4f7-08d907680a85"
  //       }
  //     },
  //     "backFillMaterial": {
  //       "type": "array",
  //       "uniqueItems": true,
  //       "title": "Back Fill Material",
  //       "items": {
  //         "$ref": "#/definitions/0b907e7d-947d-4d62-e4f1-08d907680a85"
  //       }
  //     },
  //     "isStandalone": {
  //       "type": "boolean",
  //       "title": "Standalone"
  //     },
  //     "regulatedHeadrest": {
  //       "type": "boolean",
  //       "title": "Regulated Headrest"
  //     },
  //     "frameMaterial": {
  //       "type": "array",
  //       "uniqueItems": true,
  //       "title": "Frame Material",
  //       "items": {
  //         "$ref": "#/definitions/699e1291-0b62-410d-e4f2-08d907680a85"
  //       }
  //     },
  //     "frameWoodType": {
  //       "type": "array",
  //       "uniqueItems": true,
  //       "title": "Frame Wood Type",
  //       "items": {
  //         "$ref": "#/definitions/4f8404ec-2fcc-40ae-e4f3-08d907680a85"
  //       }
  //     },
  //     "storageContainers": {
  //       "type": "boolean",
  //       "title": "Storage Containers"
  //     },
  //     "storageContainersNumber": {
  //       "type": "number",
  //       "title": "Number of Storage Containers"
  //     },
  //     "fireResistant": {
  //       "type": "boolean",
  //       "title": "Fire Resistant"
  //     },
  //     "approvedUse": {
  //       "type": "array",
  //       "uniqueItems": true,
  //       "title": "Approved Use",
  //       "items": {
  //         "$ref": "#/definitions/3c1a2fba-ca0e-49da-e4f9-08d907680a85"
  //       }
  //     },
  //     "piecesNumber": {
  //       "type": "number",
  //       "title": "Number of Pieces"
  //     },
  //     "totalWeight": {
  //       "type": "number",
  //       "title": "Total Weight [kg]"
  //     },
  //     "adultAssemblyRequired": {
  //       "type": "boolean",
  //       "title": "Adult Assembly Required"
  //     },
  //     "assemblyLevel": {
  //       "$ref": "#/definitions/50113561-2f26-4695-e4fa-08d907680a85",
  //       "title": "Level of Assembly"
  //     },
  //     "adultAssemblyNumber": {
  //       "type": "number",
  //       "title": "Number of Adults for Assembly"
  //     },
  //     "warranty": {
  //       "$ref": "#/definitions/560e339a-1ef0-401e-e4fb-08d907680a85",
  //       "title": "Warranty"
  //     },
  //     "countryOfOrigin": {
  //       "$ref": "#/definitions/a92b072d-149c-4e09-e4ed-08d907680a85",
  //       "title": "Country of Origin"
  //     }
  //   }
  // }`
};

export const ProductCardPageStory = () => <ProductCard header={header} menuTiles={menuTiles} footer={footer} productCardForm={componentProps} />;

ProductCardPageStory.story = {
  name: "Product Card Page",
};

const SellerProductCardStories = {
  title: "SellerPortal.Products",
  component: ProductCardPageStory,
};

export default SellerProductCardStories;
