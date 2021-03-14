import React, { useContext } from "react";
import { toast } from "react-toastify";
import PropTypes from "prop-types";
import NoSsr from '@material-ui/core/NoSsr';
import Autocomplete from "@material-ui/lab/Autocomplete";
import { Context } from "../../../../../../shared/stores/Store";
import useForm from "../../../../../../shared/helpers/forms/useForm";
import { TextField, Button, CircularProgress, FormControlLabel, Switch } from "@material-ui/core";
import MediaCloud from "../../../../../../shared/components/MediaCloud/MediaCloud";
import DynamicForm from "../../../../../../shared/components/DynamicForm/DynamicForm";

function ProductForm(props) {

    const uiSchema = {};

    const jsonSchema = {
        "definitions": {
          "7ce2f50f-a659-487e-811e-08d8e72d92a1": {
            "title": "Color",
            "type": "string",
            "anyOf": [
              {
                "type": "string",
                "enum": [
                  "1d755704-6608-418a-33a8-08d8e72d932d"
                ],
                "title": "Beech Wood"
              },
              {
                "type": "string",
                "enum": [
                  "fead9bf3-1500-4f7f-33a9-08d8e72d932d"
                ],
                "title": "Beige"
              },
              {
                "type": "string",
                "enum": [
                  "137d5843-b455-41f9-33aa-08d8e72d932d"
                ],
                "title": "Black"
              },
              {
                "type": "string",
                "enum": [
                  "adfd498f-406d-4944-33ab-08d8e72d932d"
                ],
                "title": "Blue"
              },
              {
                "type": "string",
                "enum": [
                  "ab93802f-0cd3-471d-33ac-08d8e72d932d"
                ],
                "title": "Brown"
              },
              {
                "type": "string",
                "enum": [
                  "839a7011-9204-4deb-33ad-08d8e72d932d"
                ],
                "title": "Brown Wood"
              },
              {
                "type": "string",
                "enum": [
                  "36746cfa-e029-4e17-33ae-08d8e72d932d"
                ],
                "title": "Colorful"
              },
              {
                "type": "string",
                "enum": [
                  "c5f32c80-0b05-4b60-33af-08d8e72d932d"
                ],
                "title": "Cream"
              },
              {
                "type": "string",
                "enum": [
                  "2aabe0fa-d37d-4a85-33b0-08d8e72d932d"
                ],
                "title": "Dark Beige"
              },
              {
                "type": "string",
                "enum": [
                  "dc068909-1140-4082-33b1-08d8e72d932d"
                ],
                "title": "Dark Black"
              },
              {
                "type": "string",
                "enum": [
                  "05ad4ed3-d320-4b0b-33b2-08d8e72d932d"
                ],
                "title": "Dark Blue"
              },
              {
                "type": "string",
                "enum": [
                  "8fec0cf9-f581-43c9-33b3-08d8e72d932d"
                ],
                "title": "Dark Brown"
              },
              {
                "type": "string",
                "enum": [
                  "f41ceb4c-8a65-4787-33b4-08d8e72d932d"
                ],
                "title": "Dark Gray"
              },
              {
                "type": "string",
                "enum": [
                  "9e92da5f-f784-419c-33b5-08d8e72d932d"
                ],
                "title": "Dark Green"
              },
              {
                "type": "string",
                "enum": [
                  "5ffbf541-aac3-4c4a-33b6-08d8e72d932d"
                ],
                "title": "Dark Purple"
              },
              {
                "type": "string",
                "enum": [
                  "97b9616d-b315-4f43-33b7-08d8e72d932d"
                ],
                "title": "Flowers"
              },
              {
                "type": "string",
                "enum": [
                  "76a8c5dd-bdf7-4cdc-33b8-08d8e72d932d"
                ],
                "title": "Gray"
              },
              {
                "type": "string",
                "enum": [
                  "c7daf83e-2737-4b8b-33b9-08d8e72d932d"
                ],
                "title": "Green"
              },
              {
                "type": "string",
                "enum": [
                  "1971dddb-5cb4-4a34-33ba-08d8e72d932d"
                ],
                "title": "Light Brown"
              },
              {
                "type": "string",
                "enum": [
                  "7ffb6b68-700e-462d-33bb-08d8e72d932d"
                ],
                "title": "Metal"
              },
              {
                "type": "string",
                "enum": [
                  "2658d857-cc5e-4bc4-33bc-08d8e72d932d"
                ],
                "title": "Orange"
              },
              {
                "type": "string",
                "enum": [
                  "38b55a6e-7c8e-4697-33bd-08d8e72d932d"
                ],
                "title": "Pink"
              },
              {
                "type": "string",
                "enum": [
                  "e6cf84ff-7b5b-4c8d-33be-08d8e72d932d"
                ],
                "title": "Purple"
              },
              {
                "type": "string",
                "enum": [
                  "dcc3557e-0f28-49ca-33bf-08d8e72d932d"
                ],
                "title": "Red"
              },
              {
                "type": "string",
                "enum": [
                  "f6fa3b5a-e844-4342-33c0-08d8e72d932d"
                ],
                "title": "Rose"
              },
              {
                "type": "string",
                "enum": [
                  "23ca813a-043a-4236-33c1-08d8e72d932d"
                ],
                "title": "Silver"
              },
              {
                "type": "string",
                "enum": [
                  "7edc742a-4f61-4034-33c2-08d8e72d932d"
                ],
                "title": "Sonoma"
              },
              {
                "type": "string",
                "enum": [
                  "c03422e7-9b79-445c-33c3-08d8e72d932d"
                ],
                "title": "Walnut Wood"
              },
              {
                "type": "string",
                "enum": [
                  "ea7d5ff1-7ed4-429a-33c4-08d8e72d932d"
                ],
                "title": "White"
              },
              {
                "type": "string",
                "enum": [
                  "7dc72005-50f3-4639-33c5-08d8e72d932d"
                ],
                "title": "Wood"
              },
              {
                "type": "string",
                "enum": [
                  "6697c235-f24f-49f9-33c6-08d8e72d932d"
                ],
                "title": "Wood Brown"
              },
              {
                "type": "string",
                "enum": [
                  "b5c189da-7a3a-473a-33c7-08d8e72d932d"
                ],
                "title": "Yellow"
              }
            ]
          },
          "194e3fba-2b3b-4795-811f-08d8e72d92a1": {
            "title": "Fabrics",
            "type": "string",
            "anyOf": [
              {
                "type": "string",
                "enum": [
                  "7e83710b-92e0-4de4-33c8-08d8e72d932d"
                ],
                "title": "Ac 01"
              },
              {
                "type": "string",
                "enum": [
                  "8bfbef8e-9479-4399-33c9-08d8e72d932d"
                ],
                "title": "Alova 04"
              },
              {
                "type": "string",
                "enum": [
                  "a7f272d9-4197-4ca7-33ca-08d8e72d932d"
                ],
                "title": "Alova 07"
              },
              {
                "type": "string",
                "enum": [
                  "ff44083a-0ea8-46b0-33cb-08d8e72d932d"
                ],
                "title": "Alova 10"
              },
              {
                "type": "string",
                "enum": [
                  "e64c9451-3344-48fa-33cc-08d8e72d932d"
                ],
                "title": "Alova 12"
              },
              {
                "type": "string",
                "enum": [
                  "1cbf94e9-a86a-48a9-33cd-08d8e72d932d"
                ],
                "title": "Alova 29"
              },
              {
                "type": "string",
                "enum": [
                  "581a4298-6c6b-449a-33ce-08d8e72d932d"
                ],
                "title": "Alova 36"
              },
              {
                "type": "string",
                "enum": [
                  "6baae712-3509-40a6-33cf-08d8e72d932d"
                ],
                "title": "Alova 42"
              },
              {
                "type": "string",
                "enum": [
                  "ad03b67e-d1de-4e08-33d0-08d8e72d932d"
                ],
                "title": "Alova 46"
              },
              {
                "type": "string",
                "enum": [
                  "775a27f5-2b08-4e64-33d1-08d8e72d932d"
                ],
                "title": "Alova 48"
              },
              {
                "type": "string",
                "enum": [
                  "3e08e785-5827-402b-33d2-08d8e72d932d"
                ],
                "title": "Alova 66"
              },
              {
                "type": "string",
                "enum": [
                  "04aa1cea-aa0a-4e7f-33d3-08d8e72d932d"
                ],
                "title": "Alova 67"
              },
              {
                "type": "string",
                "enum": [
                  "a965c3ea-b6f0-494c-33d4-08d8e72d932d"
                ],
                "title": "Alova 68"
              },
              {
                "type": "string",
                "enum": [
                  "581334d8-708d-4144-33d5-08d8e72d932d"
                ],
                "title": "Alova 76"
              },
              {
                "type": "string",
                "enum": [
                  "a8575bec-7648-45cc-33d6-08d8e72d932d"
                ],
                "title": "Alova 79"
              },
              {
                "type": "string",
                "enum": [
                  "f8e18759-d58d-4d81-33d7-08d8e72d932d"
                ],
                "title": "Alova Pdp"
              },
              {
                "type": "string",
                "enum": [
                  "29619c8e-88b8-430c-33d8-08d8e72d932d"
                ],
                "title": "Arte 51B"
              },
              {
                "type": "string",
                "enum": [
                  "e7a5d9d1-bf89-4dca-33d9-08d8e72d932d"
                ],
                "title": "Arte 80A"
              },
              {
                "type": "string",
                "enum": [
                  "77d8be6f-fafe-46c6-33da-08d8e72d932d"
                ],
                "title": "Berlin 01"
              },
              {
                "type": "string",
                "enum": [
                  "12f9f3bc-c190-4181-33db-08d8e72d932d"
                ],
                "title": "Berlin 02"
              },
              {
                "type": "string",
                "enum": [
                  "eeaa0c66-793b-4ee0-33dc-08d8e72d932d"
                ],
                "title": "Berlin 03"
              },
              {
                "type": "string",
                "enum": [
                  "e71e1fdf-2374-4f86-33dd-08d8e72d932d"
                ],
                "title": "Berlin 10"
              },
              {
                "type": "string",
                "enum": [
                  "700be956-1c34-430b-33de-08d8e72d932d"
                ],
                "title": "Beton"
              },
              {
                "type": "string",
                "enum": [
                  "f3d76a94-7521-4b07-33df-08d8e72d932d"
                ],
                "title": "Botanical 80"
              },
              {
                "type": "string",
                "enum": [
                  "94f6befd-9482-4fb9-33e0-08d8e72d932d"
                ],
                "title": "Butterfly 04"
              },
              {
                "type": "string",
                "enum": [
                  "f4cb1329-6f86-40dd-33e1-08d8e72d932d"
                ],
                "title": "Cover 02"
              },
              {
                "type": "string",
                "enum": [
                  "ee5cb9d8-2602-43b2-33e2-08d8e72d932d"
                ],
                "title": "Cover 61"
              },
              {
                "type": "string",
                "enum": [
                  "e05f4e53-a08d-4ff3-33e3-08d8e72d932d"
                ],
                "title": "Cover 70"
              },
              {
                "type": "string",
                "enum": [
                  "af5eee9c-f2d2-4bc0-33e4-08d8e72d932d"
                ],
                "title": "Cover 83"
              },
              {
                "type": "string",
                "enum": [
                  "28fd4726-4486-4e51-33e5-08d8e72d932d"
                ],
                "title": "Cover 87"
              },
              {
                "type": "string",
                "enum": [
                  "3121499e-bf58-4926-33e6-08d8e72d932d"
                ],
                "title": "Dora 21"
              },
              {
                "type": "string",
                "enum": [
                  "72ef1043-07d0-4979-33e7-08d8e72d932d"
                ],
                "title": "Dora 22"
              },
              {
                "type": "string",
                "enum": [
                  "55564483-a803-4545-33e8-08d8e72d932d"
                ],
                "title": "Dora 26"
              },
              {
                "type": "string",
                "enum": [
                  "3f8328dd-533d-4bd2-33e9-08d8e72d932d"
                ],
                "title": "Dora 28"
              },
              {
                "type": "string",
                "enum": [
                  "cae6792e-cbd4-43d6-33ea-08d8e72d932d"
                ],
                "title": "Dora 63"
              },
              {
                "type": "string",
                "enum": [
                  "a7883585-a69a-432c-33eb-08d8e72d932d"
                ],
                "title": "Dora 85"
              },
              {
                "type": "string",
                "enum": [
                  "29529238-cc23-4776-33ec-08d8e72d932d"
                ],
                "title": "Dora 90"
              },
              {
                "type": "string",
                "enum": [
                  "a4c0754d-b2bb-434a-33ed-08d8e72d932d"
                ],
                "title": "Dora 95"
              },
              {
                "type": "string",
                "enum": [
                  "16d6165d-19f6-4fe7-33ee-08d8e72d932d"
                ],
                "title": "Dora 96"
              },
              {
                "type": "string",
                "enum": [
                  "4e8986f5-72f5-4df8-33ef-08d8e72d932d"
                ],
                "title": "Garden 39"
              },
              {
                "type": "string",
                "enum": [
                  "d7cf579e-92e9-48be-33f0-08d8e72d932d"
                ],
                "title": "Glamour 38"
              },
              {
                "type": "string",
                "enum": [
                  "32216ce7-77e0-4381-33f1-08d8e72d932d"
                ],
                "title": "Grande 39"
              },
              {
                "type": "string",
                "enum": [
                  "4d439219-d8e0-4c41-33f2-08d8e72d932d"
                ],
                "title": "Grande 75"
              },
              {
                "type": "string",
                "enum": [
                  "7a0f67a1-d86b-4349-33f3-08d8e72d932d"
                ],
                "title": "Grande 77"
              },
              {
                "type": "string",
                "enum": [
                  "3499e22c-bd58-413c-33f4-08d8e72d932d"
                ],
                "title": "Grande 81"
              },
              {
                "type": "string",
                "enum": [
                  "646292f4-266f-44ee-33f5-08d8e72d932d"
                ],
                "title": "Grande 97"
              },
              {
                "type": "string",
                "enum": [
                  "e62f1b22-fb9d-48cc-33f6-08d8e72d932d"
                ],
                "title": "Gusto 61"
              },
              {
                "type": "string",
                "enum": [
                  "940dd017-c5bd-45eb-33f7-08d8e72d932d"
                ],
                "title": "Gusto 69"
              },
              {
                "type": "string",
                "enum": [
                  "1c6b57c7-12be-49a0-33f8-08d8e72d932d"
                ],
                "title": "Gusto 82"
              },
              {
                "type": "string",
                "enum": [
                  "3103a865-ed9b-4487-33f9-08d8e72d932d"
                ],
                "title": "Gusto 88"
              },
              {
                "type": "string",
                "enum": [
                  "b79d9abc-2299-41a1-33fa-08d8e72d932d"
                ],
                "title": "Inari 100"
              },
              {
                "type": "string",
                "enum": [
                  "4e71385b-4a7f-4cf3-33fb-08d8e72d932d"
                ],
                "title": "Inari 23"
              },
              {
                "type": "string",
                "enum": [
                  "1e32368e-1668-4310-33fc-08d8e72d932d"
                ],
                "title": "Inari 28"
              },
              {
                "type": "string",
                "enum": [
                  "1db470a7-df72-4499-33fd-08d8e72d932d"
                ],
                "title": "Inari 80"
              },
              {
                "type": "string",
                "enum": [
                  "5d272a7e-d3a9-443e-33fe-08d8e72d932d"
                ],
                "title": "Inari 91"
              },
              {
                "type": "string",
                "enum": [
                  "e69f018c-7090-4787-33ff-08d8e72d932d"
                ],
                "title": "Inari 96"
              },
              {
                "type": "string",
                "enum": [
                  "8ca2ade3-07e2-4caa-3400-08d8e72d932d"
                ],
                "title": "Jasmine 22"
              },
              {
                "type": "string",
                "enum": [
                  "a40237c3-de46-4fd1-3401-08d8e72d932d"
                ],
                "title": "Jasmine 29"
              },
              {
                "type": "string",
                "enum": [
                  "c9a312ac-f0c2-4143-3402-08d8e72d932d"
                ],
                "title": "Jasmine 73"
              },
              {
                "type": "string",
                "enum": [
                  "734fed55-4396-4303-3403-08d8e72d932d"
                ],
                "title": "Jasmine 85"
              },
              {
                "type": "string",
                "enum": [
                  "81a55664-785f-4912-3404-08d8e72d932d"
                ],
                "title": "Jasmine 90"
              },
              {
                "type": "string",
                "enum": [
                  "676075d1-6373-4cf0-3405-08d8e72d932d"
                ],
                "title": "Jasmine 96"
              },
              {
                "type": "string",
                "enum": [
                  "a855cab2-ea40-402f-3406-08d8e72d932d"
                ],
                "title": "Jungle 32"
              },
              {
                "type": "string",
                "enum": [
                  "cc0e96f1-0943-428f-3407-08d8e72d932d"
                ],
                "title": "Jungle 37"
              },
              {
                "type": "string",
                "enum": [
                  "7bba5e57-894b-49a2-3408-08d8e72d932d"
                ],
                "title": "Kronos 02"
              },
              {
                "type": "string",
                "enum": [
                  "35cedeea-9ed4-4a51-3409-08d8e72d932d"
                ],
                "title": "Kronos 06"
              },
              {
                "type": "string",
                "enum": [
                  "dbe14141-0832-4640-340a-08d8e72d932d"
                ],
                "title": "Kronos 07"
              },
              {
                "type": "string",
                "enum": [
                  "5d40d68b-f876-4c83-340b-08d8e72d932d"
                ],
                "title": "Kronos 09"
              },
              {
                "type": "string",
                "enum": [
                  "57ce2557-f6ae-4cd2-340c-08d8e72d932d"
                ],
                "title": "Kronos 13"
              },
              {
                "type": "string",
                "enum": [
                  "c480bf4b-cc60-4406-340d-08d8e72d932d"
                ],
                "title": "Kronos 17"
              },
              {
                "type": "string",
                "enum": [
                  "9b7cc6d2-aadb-4a32-340e-08d8e72d932d"
                ],
                "title": "Kronos 19"
              },
              {
                "type": "string",
                "enum": [
                  "fc2fc526-4e64-469d-340f-08d8e72d932d"
                ],
                "title": "Kronos 29"
              },
              {
                "type": "string",
                "enum": [
                  "7b1248d2-7eb2-407f-3410-08d8e72d932d"
                ],
                "title": "Lars 68"
              },
              {
                "type": "string",
                "enum": [
                  "0c591406-5df7-4c76-3411-08d8e72d932d"
                ],
                "title": "Lars 90"
              },
              {
                "type": "string",
                "enum": [
                  "53718600-9345-4b53-3412-08d8e72d932d"
                ],
                "title": "Lars 98"
              },
              {
                "type": "string",
                "enum": [
                  "a1ccd87f-d434-477f-3413-08d8e72d932d"
                ],
                "title": "Lars 99"
              },
              {
                "type": "string",
                "enum": [
                  "51549958-2dab-4226-3414-08d8e72d932d"
                ],
                "title": "Lastrico 2_76"
              },
              {
                "type": "string",
                "enum": [
                  "4ec58276-aff3-4fac-3415-08d8e72d932d"
                ],
                "title": "Lima 67"
              },
              {
                "type": "string",
                "enum": [
                  "0dafc739-a04d-4beb-3416-08d8e72d932d"
                ],
                "title": "Lima 75"
              },
              {
                "type": "string",
                "enum": [
                  "47322a8f-127a-479c-3417-08d8e72d932d"
                ],
                "title": "Madison 3_79"
              },
              {
                "type": "string",
                "enum": [
                  "38b8e01a-f7bd-4b25-3418-08d8e72d932d"
                ],
                "title": "Malmo 05"
              },
              {
                "type": "string",
                "enum": [
                  "d6a5a36f-d679-47b7-3419-08d8e72d932d"
                ],
                "title": "Malmo 23"
              },
              {
                "type": "string",
                "enum": [
                  "4bde4245-5004-43de-341a-08d8e72d932d"
                ],
                "title": "Malmo 26"
              },
              {
                "type": "string",
                "enum": [
                  "f7fbf596-26e8-453d-341b-08d8e72d932d"
                ],
                "title": "Malmo 37"
              },
              {
                "type": "string",
                "enum": [
                  "d06a47ec-2a6a-48c0-341c-08d8e72d932d"
                ],
                "title": "Malmo 41"
              },
              {
                "type": "string",
                "enum": [
                  "c4552c01-039f-4289-341d-08d8e72d932d"
                ],
                "title": "Malmo 61"
              },
              {
                "type": "string",
                "enum": [
                  "6ed8cc5e-b3ef-4bfa-341e-08d8e72d932d"
                ],
                "title": "Malmo 63"
              },
              {
                "type": "string",
                "enum": [
                  "fdf10f9c-be65-4b5b-341f-08d8e72d932d"
                ],
                "title": "Malmo 72"
              },
              {
                "type": "string",
                "enum": [
                  "48cb06bd-ace3-4604-3420-08d8e72d932d"
                ],
                "title": "Malmo 79"
              },
              {
                "type": "string",
                "enum": [
                  "753d700b-4140-485b-3421-08d8e72d932d"
                ],
                "title": "Malmo 81"
              },
              {
                "type": "string",
                "enum": [
                  "5c2f5cac-8d6d-4789-3422-08d8e72d932d"
                ],
                "title": "Malmo 83"
              },
              {
                "type": "string",
                "enum": [
                  "16b84526-d590-4d1e-3423-08d8e72d932d"
                ],
                "title": "Malmo 85"
              },
              {
                "type": "string",
                "enum": [
                  "fcfa6ab7-25c7-4690-3424-08d8e72d932d"
                ],
                "title": "Malmo 90"
              },
              {
                "type": "string",
                "enum": [
                  "d83ff741-aaf6-43c1-3425-08d8e72d932d"
                ],
                "title": "Malmo 95"
              },
              {
                "type": "string",
                "enum": [
                  "8c0cc404-5ea5-49f3-3426-08d8e72d932d"
                ],
                "title": "Malmo 96"
              },
              {
                "type": "string",
                "enum": [
                  "5939221b-c3a8-4074-3427-08d8e72d932d"
                ],
                "title": "Mat Velvet 29"
              },
              {
                "type": "string",
                "enum": [
                  "dfbd747a-d9d2-48c2-3428-08d8e72d932d"
                ],
                "title": "Mat Velvet 63"
              },
              {
                "type": "string",
                "enum": [
                  "e968159b-9d9f-4127-3429-08d8e72d932d"
                ],
                "title": "Mat Velvet 68"
              },
              {
                "type": "string",
                "enum": [
                  "37eb554c-cf75-4061-342a-08d8e72d932d"
                ],
                "title": "Mat Velvet 75"
              },
              {
                "type": "string",
                "enum": [
                  "e546c4ef-468c-4a6b-342b-08d8e72d932d"
                ],
                "title": "Mat Velvet 85"
              },
              {
                "type": "string",
                "enum": [
                  "236dab8f-05e1-40ff-342c-08d8e72d932d"
                ],
                "title": "Mat Velvet 99"
              },
              {
                "type": "string",
                "enum": [
                  "fe430bf0-7da2-44fc-342d-08d8e72d932d"
                ],
                "title": "Microfibre"
              },
              {
                "type": "string",
                "enum": [
                  "ae3ff1c2-d320-4efb-342e-08d8e72d932d"
                ],
                "title": "Monet 95"
              },
              {
                "type": "string",
                "enum": [
                  "54e0744d-dc24-46f8-342f-08d8e72d932d"
                ],
                "title": "Monolith 09"
              },
              {
                "type": "string",
                "enum": [
                  "788ec020-8fe4-48f0-3430-08d8e72d932d"
                ],
                "title": "Monolith 29"
              },
              {
                "type": "string",
                "enum": [
                  "468956cc-d3f7-471e-3431-08d8e72d932d"
                ],
                "title": "Monolith 37"
              },
              {
                "type": "string",
                "enum": [
                  "cd9d800e-7af7-43dc-3432-08d8e72d932d"
                ],
                "title": "Monolith 38"
              },
              {
                "type": "string",
                "enum": [
                  "e14241df-3370-4eb6-3433-08d8e72d932d"
                ],
                "title": "Monolith 48"
              },
              {
                "type": "string",
                "enum": [
                  "92ca35e4-403d-4517-3434-08d8e72d932d"
                ],
                "title": "Monolith 63"
              },
              {
                "type": "string",
                "enum": [
                  "4380ded6-33b7-4609-3435-08d8e72d932d"
                ],
                "title": "Monolith 77"
              },
              {
                "type": "string",
                "enum": [
                  "8f88df50-b57f-400b-3436-08d8e72d932d"
                ],
                "title": "Monolith 83"
              },
              {
                "type": "string",
                "enum": [
                  "3376f72f-ce50-4c42-3437-08d8e72d932d"
                ],
                "title": "Monolith 84"
              },
              {
                "type": "string",
                "enum": [
                  "92d107ec-09fd-4911-3438-08d8e72d932d"
                ],
                "title": "Monolith 85"
              },
              {
                "type": "string",
                "enum": [
                  "688deefc-f75a-4e9c-3439-08d8e72d932d"
                ],
                "title": "Monolith 97"
              },
              {
                "type": "string",
                "enum": [
                  "bf0462c3-2928-46b8-343a-08d8e72d932d"
                ],
                "title": "Nubuk 11"
              },
              {
                "type": "string",
                "enum": [
                  "4448c839-4fad-4695-343b-08d8e72d932d"
                ],
                "title": "Nubuk 132"
              },
              {
                "type": "string",
                "enum": [
                  "23a6c86d-def6-4fdd-343c-08d8e72d932d"
                ],
                "title": "Nubuk 16"
              },
              {
                "type": "string",
                "enum": [
                  "c0aa5f0f-7ab2-4115-343d-08d8e72d932d"
                ],
                "title": "Nubuk 2014"
              },
              {
                "type": "string",
                "enum": [
                  "be91c0aa-99db-44eb-343e-08d8e72d932d"
                ],
                "title": "Nubuk 21"
              },
              {
                "type": "string",
                "enum": [
                  "29e64b4b-fa2e-46ad-343f-08d8e72d932d"
                ],
                "title": "Nubuk 27"
              },
              {
                "type": "string",
                "enum": [
                  "a76e0a7d-0e1a-4567-3440-08d8e72d932d"
                ],
                "title": "Nubuk 66"
              },
              {
                "type": "string",
                "enum": [
                  "b089662d-01f9-4524-3441-08d8e72d932d"
                ],
                "title": "Nut Burgundia"
              },
              {
                "type": "string",
                "enum": [
                  "6077e4a4-c21c-4f2d-3442-08d8e72d932d"
                ],
                "title": "Omega 02"
              },
              {
                "type": "string",
                "enum": [
                  "acbb907a-3e44-4980-3443-08d8e72d932d"
                ],
                "title": "Omega 13"
              },
              {
                "type": "string",
                "enum": [
                  "84cba762-50ca-42b9-3444-08d8e72d932d"
                ],
                "title": "Omega 68"
              },
              {
                "type": "string",
                "enum": [
                  "58a32b44-9b43-4d4d-3445-08d8e72d932d"
                ],
                "title": "Omega 86"
              },
              {
                "type": "string",
                "enum": [
                  "b52403b4-d95c-4838-3446-08d8e72d932d"
                ],
                "title": "Omega 91"
              },
              {
                "type": "string",
                "enum": [
                  "69f5b678-0aec-43bb-3447-08d8e72d932d"
                ],
                "title": "Ontario 100"
              },
              {
                "type": "string",
                "enum": [
                  "2b61d506-569c-4f1d-3448-08d8e72d932d"
                ],
                "title": "Ontario 22"
              },
              {
                "type": "string",
                "enum": [
                  "93e649fe-57f3-4188-3449-08d8e72d932d"
                ],
                "title": "Ontario 40"
              },
              {
                "type": "string",
                "enum": [
                  "3fb0816f-8c0a-4a04-344a-08d8e72d932d"
                ],
                "title": "Ontario 65"
              },
              {
                "type": "string",
                "enum": [
                  "dbec1f71-7ae0-4a80-344b-08d8e72d932d"
                ],
                "title": "Ontario 81"
              },
              {
                "type": "string",
                "enum": [
                  "4d65c874-ee4d-4d00-344c-08d8e72d932d"
                ],
                "title": "Ontario 83"
              },
              {
                "type": "string",
                "enum": [
                  "38d00f39-6631-406b-344d-08d8e72d932d"
                ],
                "title": "Ontario 91"
              },
              {
                "type": "string",
                "enum": [
                  "e16d45a9-b4f1-4de6-344e-08d8e72d932d"
                ],
                "title": "Ontario 96"
              },
              {
                "type": "string",
                "enum": [
                  "37afde3a-cba7-4952-344f-08d8e72d932d"
                ],
                "title": "Orinoco 100"
              },
              {
                "type": "string",
                "enum": [
                  "66927a0c-057f-4f83-3450-08d8e72d932d"
                ],
                "title": "Orinoco 21"
              },
              {
                "type": "string",
                "enum": [
                  "702aefe2-01db-4044-3451-08d8e72d932d"
                ],
                "title": "Orinoco 22"
              },
              {
                "type": "string",
                "enum": [
                  "ba230a32-ad6d-4b56-3452-08d8e72d932d"
                ],
                "title": "Orinoco 29"
              },
              {
                "type": "string",
                "enum": [
                  "37a48227-1d81-4076-3453-08d8e72d932d"
                ],
                "title": "Orinoco 65"
              },
              {
                "type": "string",
                "enum": [
                  "42de2bd3-f133-4fe7-3454-08d8e72d932d"
                ],
                "title": "Orinoco 80"
              },
              {
                "type": "string",
                "enum": [
                  "a9eef7a8-dfc5-42e3-3455-08d8e72d932d"
                ],
                "title": "Orinoco 85"
              },
              {
                "type": "string",
                "enum": [
                  "4df8c01d-ec69-4bd6-3456-08d8e72d932d"
                ],
                "title": "Orinoco 96"
              },
              {
                "type": "string",
                "enum": [
                  "c140071b-927e-4f39-3457-08d8e72d932d"
                ],
                "title": "Palacio 77"
              },
              {
                "type": "string",
                "enum": [
                  "6b0dfd02-dbc0-48dc-3458-08d8e72d932d"
                ],
                "title": "Paros 02"
              },
              {
                "type": "string",
                "enum": [
                  "5bcb1633-3d39-4c31-3459-08d8e72d932d"
                ],
                "title": "Paros 05"
              },
              {
                "type": "string",
                "enum": [
                  "4a4c011c-5a95-4f93-345a-08d8e72d932d"
                ],
                "title": "Paros 06"
              },
              {
                "type": "string",
                "enum": [
                  "fe224a00-115a-4151-345b-08d8e72d932d"
                ],
                "title": "Portland 90"
              },
              {
                "type": "string",
                "enum": [
                  "29c33f6f-12a1-4b7c-345c-08d8e72d932d"
                ],
                "title": "Portland 95"
              },
              {
                "type": "string",
                "enum": [
                  "7545a522-4d07-494d-345d-08d8e72d932d"
                ],
                "title": "Primo 89"
              },
              {
                "type": "string",
                "enum": [
                  "c814793a-3e91-4bbc-345e-08d8e72d932d"
                ],
                "title": "Primo 96"
              },
              {
                "type": "string",
                "enum": [
                  "eab3d80e-7ce2-4057-345f-08d8e72d932d"
                ],
                "title": "Rivera 100"
              },
              {
                "type": "string",
                "enum": [
                  "f73f1416-0ad9-4f71-3460-08d8e72d932d"
                ],
                "title": "Rivera 36"
              },
              {
                "type": "string",
                "enum": [
                  "1d75bafc-23d4-4bd0-3461-08d8e72d932d"
                ],
                "title": "Rivera 59"
              },
              {
                "type": "string",
                "enum": [
                  "3c0823c6-3bd8-45d6-3462-08d8e72d932d"
                ],
                "title": "Rose 14"
              },
              {
                "type": "string",
                "enum": [
                  "dee0644c-0c30-4a53-3463-08d8e72d932d"
                ],
                "title": "Sawana 01"
              },
              {
                "type": "string",
                "enum": [
                  "abcdc81b-6dda-4d8a-3464-08d8e72d932d"
                ],
                "title": "Sawana 05"
              },
              {
                "type": "string",
                "enum": [
                  "607ba634-9783-453a-3465-08d8e72d932d"
                ],
                "title": "Sawana 14"
              },
              {
                "type": "string",
                "enum": [
                  "1de482a1-6145-4d4a-3466-08d8e72d932d"
                ],
                "title": "Sawana 16"
              },
              {
                "type": "string",
                "enum": [
                  "0e75bdda-7cf0-463a-3467-08d8e72d932d"
                ],
                "title": "Sawana 21"
              },
              {
                "type": "string",
                "enum": [
                  "ce3e84a3-5587-40b7-3468-08d8e72d932d"
                ],
                "title": "Sawana 25"
              },
              {
                "type": "string",
                "enum": [
                  "9789bce1-2ed8-4615-3469-08d8e72d932d"
                ],
                "title": "Sawana 26"
              },
              {
                "type": "string",
                "enum": [
                  "4515539b-673c-49ef-346a-08d8e72d932d"
                ],
                "title": "Sawana 72"
              },
              {
                "type": "string",
                "enum": [
                  "200db0ed-3e93-4459-346b-08d8e72d932d"
                ],
                "title": "Sawana 80"
              },
              {
                "type": "string",
                "enum": [
                  "16917c29-eb17-4b91-346c-08d8e72d932d"
                ],
                "title": "Sawana 84"
              },
              {
                "type": "string",
                "enum": [
                  "f8db70eb-963a-42e4-346d-08d8e72d932d"
                ],
                "title": "Soft 09"
              },
              {
                "type": "string",
                "enum": [
                  "174f96d5-97d3-4c87-346e-08d8e72d932d"
                ],
                "title": "Soft 10"
              },
              {
                "type": "string",
                "enum": [
                  "123dd771-059e-4262-346f-08d8e72d932d"
                ],
                "title": "Soft 11"
              },
              {
                "type": "string",
                "enum": [
                  "9ce8def0-8850-4bca-3470-08d8e72d932d"
                ],
                "title": "Soft 15"
              },
              {
                "type": "string",
                "enum": [
                  "0a8faeab-a4da-40c2-3471-08d8e72d932d"
                ],
                "title": "Soft 16"
              },
              {
                "type": "string",
                "enum": [
                  "720dbab5-5200-4166-3472-08d8e72d932d"
                ],
                "title": "Soft 17"
              },
              {
                "type": "string",
                "enum": [
                  "8176cc10-d2d3-445d-3473-08d8e72d932d"
                ],
                "title": "Soft 18"
              },
              {
                "type": "string",
                "enum": [
                  "60688e31-bcf2-4d04-3474-08d8e72d932d"
                ],
                "title": "Soft 29"
              },
              {
                "type": "string",
                "enum": [
                  "c6e93bc6-6e3c-4416-3475-08d8e72d932d"
                ],
                "title": "Soft 33"
              },
              {
                "type": "string",
                "enum": [
                  "ea5999ba-0fa5-484f-3476-08d8e72d932d"
                ],
                "title": "Soft 66"
              },
              {
                "type": "string",
                "enum": [
                  "3e614131-ceaf-4a6c-3477-08d8e72d932d"
                ],
                "title": "Soft 929"
              },
              {
                "type": "string",
                "enum": [
                  "6986f0cb-013e-4e8f-3478-08d8e72d932d"
                ],
                "title": "Soft 985"
              },
              {
                "type": "string",
                "enum": [
                  "9bbe18bc-fe5a-4b97-3479-08d8e72d932d"
                ],
                "title": "Solar 16"
              },
              {
                "type": "string",
                "enum": [
                  "6b14669d-7db4-4651-347a-08d8e72d932d"
                ],
                "title": "Solar 63"
              },
              {
                "type": "string",
                "enum": [
                  "8bcc8a83-278a-41ff-347b-08d8e72d932d"
                ],
                "title": "Solar 70"
              },
              {
                "type": "string",
                "enum": [
                  "e3bdae46-43db-4ac8-347c-08d8e72d932d"
                ],
                "title": "Solar 79"
              },
              {
                "type": "string",
                "enum": [
                  "c89c1625-0da5-4941-347d-08d8e72d932d"
                ],
                "title": "Solar 80"
              },
              {
                "type": "string",
                "enum": [
                  "4f6d0de5-76d8-4a1a-347e-08d8e72d932d"
                ],
                "title": "Solar 99"
              },
              {
                "type": "string",
                "enum": [
                  "27cfd7b0-d1c7-4c7c-347f-08d8e72d932d"
                ],
                "title": "Solid 09"
              },
              {
                "type": "string",
                "enum": [
                  "da9b23ae-3e2b-484e-3480-08d8e72d932d"
                ],
                "title": "Solid 39"
              },
              {
                "type": "string",
                "enum": [
                  "09d78c46-85cb-44fe-3481-08d8e72d932d"
                ],
                "title": "Solid 77"
              },
              {
                "type": "string",
                "enum": [
                  "4f57d994-7a3c-4fea-3482-08d8e72d932d"
                ],
                "title": "Sonoma"
              },
              {
                "type": "string",
                "enum": [
                  "2bbc6f71-7528-4059-3483-08d8e72d932d"
                ],
                "title": "Soro 100"
              },
              {
                "type": "string",
                "enum": [
                  "7f87d1f6-c549-4815-3484-08d8e72d932d"
                ],
                "title": "Soro 13"
              },
              {
                "type": "string",
                "enum": [
                  "43dca8ad-f19f-44cd-3485-08d8e72d932d"
                ],
                "title": "Soro 28"
              },
              {
                "type": "string",
                "enum": [
                  "cba32790-a8fe-468a-3486-08d8e72d932d"
                ],
                "title": "Soro 34"
              },
              {
                "type": "string",
                "enum": [
                  "17fadaa2-b765-4e53-3487-08d8e72d932d"
                ],
                "title": "Soro 40"
              },
              {
                "type": "string",
                "enum": [
                  "b2e4184f-1b37-4606-3488-08d8e72d932d"
                ],
                "title": "Soro 61"
              },
              {
                "type": "string",
                "enum": [
                  "1d4bf428-f9ee-4eda-3489-08d8e72d932d"
                ],
                "title": "Soro 65"
              },
              {
                "type": "string",
                "enum": [
                  "554c4102-ba5b-4d79-348a-08d8e72d932d"
                ],
                "title": "Soro 76"
              },
              {
                "type": "string",
                "enum": [
                  "b1f42523-ba9c-4dcc-348b-08d8e72d932d"
                ],
                "title": "Soro 83"
              },
              {
                "type": "string",
                "enum": [
                  "2bfe478a-5adf-4f21-348c-08d8e72d932d"
                ],
                "title": "Soro 93"
              },
              {
                "type": "string",
                "enum": [
                  "43532c1c-ae5e-42db-348d-08d8e72d932d"
                ],
                "title": "Soro 95"
              },
              {
                "type": "string",
                "enum": [
                  "f0b81fda-550e-4ea8-348e-08d8e72d932d"
                ],
                "title": "Texas 23"
              },
              {
                "type": "string",
                "enum": [
                  "3cab67a2-d3eb-4686-348f-08d8e72d932d"
                ],
                "title": "Texas 28"
              },
              {
                "type": "string",
                "enum": [
                  "e8735185-d689-46fd-3490-08d8e72d932d"
                ],
                "title": "Texas 92"
              },
              {
                "type": "string",
                "enum": [
                  "18b2e9bd-1335-4d3b-3491-08d8e72d932d"
                ],
                "title": "Vernal 61"
              },
              {
                "type": "string",
                "enum": [
                  "c86b7210-31bd-415e-3492-08d8e72d932d"
                ],
                "title": "White Mat"
              },
              {
                "type": "string",
                "enum": [
                  "1f76f917-4608-4c5d-3493-08d8e72d932d"
                ],
                "title": "Zigzag 53"
              },
              {
                "type": "string",
                "enum": [
                  "1d849da1-7ecc-4ea0-3494-08d8e72d932d"
                ],
                "title": "Zigzag 60"
              }
            ]
          },
          "a88b5228-7996-40b0-8121-08d8e72d92a1": {
            "title": "Shape",
            "type": "string",
            "anyOf": [
              {
                "type": "string",
                "enum": [
                  "9c15cf5a-98a2-404a-3496-08d8e72d932d"
                ],
                "title": "L"
              },
              {
                "type": "string",
                "enum": [
                  "75392bd3-e0c8-41be-3497-08d8e72d932d"
                ],
                "title": "U"
              }
            ]
          },
          "8a88cb57-46ea-4c3c-8122-08d8e72d92a1": {
            "title": "Orientation",
            "type": "string",
            "anyOf": [
              {
                "type": "string",
                "enum": [
                  "d8649845-eaa3-4caa-3498-08d8e72d932d"
                ],
                "title": "Left"
              },
              {
                "type": "string",
                "enum": [
                  "10c724f0-d078-40dc-3499-08d8e72d932d"
                ],
                "title": "Reversible"
              },
              {
                "type": "string",
                "enum": [
                  "f9491589-8d57-45aa-349a-08d8e72d932d"
                ],
                "title": "Right"
              },
              {
                "type": "string",
                "enum": [
                  "bb5ce7db-871c-426e-349b-08d8e72d932d"
                ],
                "title": "Symmetrical"
              }
            ]
          },
          "9dc2c5d6-cf0d-4b17-8123-08d8e72d92a1": {
            "title": "Upholstery Material",
            "type": "string",
            "anyOf": [
              {
                "type": "string",
                "enum": [
                  "4f6d2812-fba6-46e3-349c-08d8e72d932d"
                ],
                "title": "Polyester"
              }
            ]
          },
          "babea455-179c-4efc-8124-08d8e72d92a1": {
            "title": "Fill Material",
            "type": "string",
            "anyOf": [
              {
                "type": "string",
                "enum": [
                  "52ca3d62-2b8b-490d-349d-08d8e72d932d"
                ],
                "title": "Armchair - Foam-T30"
              },
              {
                "type": "string",
                "enum": [
                  "55efa785-6d8c-475a-349e-08d8e72d932d"
                ],
                "title": "Foam"
              },
              {
                "type": "string",
                "enum": [
                  "1478d4f1-c342-42d7-349f-08d8e72d932d"
                ],
                "title": "Foam- High Quality Hr+ T3030"
              },
              {
                "type": "string",
                "enum": [
                  "f5e38295-9942-4e91-34a0-08d8e72d932d"
                ],
                "title": "Foam- Hr"
              },
              {
                "type": "string",
                "enum": [
                  "6678bb58-3dba-4163-34a1-08d8e72d932d"
                ],
                "title": "Foam Hr + Multipocket Spring + Felt + Latex"
              },
              {
                "type": "string",
                "enum": [
                  "4a13bcee-8ac0-44da-34a2-08d8e72d932d"
                ],
                "title": "Foam Hr + Pocket Spring + Felt + Latex"
              },
              {
                "type": "string",
                "enum": [
                  "012609cf-e778-49e5-34a3-08d8e72d932d"
                ],
                "title": "Foam- Hr+T21"
              },
              {
                "type": "string",
                "enum": [
                  "3c949257-265d-48f0-34a4-08d8e72d932d"
                ],
                "title": "Foam Latex"
              },
              {
                "type": "string",
                "enum": [
                  "d6534d53-06d4-482e-34a5-08d8e72d932d"
                ],
                "title": "Foam Pur-Fabric"
              },
              {
                "type": "string",
                "enum": [
                  "3d5f31bc-582d-48ad-34a6-08d8e72d932d"
                ],
                "title": "Foam- T25"
              },
              {
                "type": "string",
                "enum": [
                  "6ddd3058-eab2-497f-34a7-08d8e72d932d"
                ],
                "title": "Foam T2541"
              },
              {
                "type": "string",
                "enum": [
                  "2f6b645e-42ae-41df-34a8-08d8e72d932d"
                ],
                "title": "Foam T2542"
              },
              {
                "type": "string",
                "enum": [
                  "bf3bc986-9592-4291-34a9-08d8e72d932d"
                ],
                "title": "Foam T2542  Latex + Coconut Mat"
              },
              {
                "type": "string",
                "enum": [
                  "9e29fc4a-1d73-4004-34aa-08d8e72d932d"
                ],
                "title": "Foam T2542 + Bonnell + Felt"
              },
              {
                "type": "string",
                "enum": [
                  "a05a8967-91fe-4935-34ab-08d8e72d932d"
                ],
                "title": "Foam T2542 + Bonnell + Felt + Coconut Mat"
              },
              {
                "type": "string",
                "enum": [
                  "5e7ebad7-5154-42ba-34ac-08d8e72d932d"
                ],
                "title": "Foam T2542 + Coconut Mat"
              },
              {
                "type": "string",
                "enum": [
                  "ae1789e0-5969-429d-34ad-08d8e72d932d"
                ],
                "title": "Foam T2542 + Latex"
              },
              {
                "type": "string",
                "enum": [
                  "d4892a6c-e423-4931-34ae-08d8e72d932d"
                ],
                "title": "Foam T2542 + Multipocket Spring + Felt"
              },
              {
                "type": "string",
                "enum": [
                  "487c9467-34eb-43a6-34af-08d8e72d932d"
                ],
                "title": "Foam T2542 + Multipocket Spring + Felt + Coconut Mat"
              },
              {
                "type": "string",
                "enum": [
                  "7e82b19f-00b6-430b-34b0-08d8e72d932d"
                ],
                "title": "Foam T2542 + Multipocket Spring + Felt + Latex"
              },
              {
                "type": "string",
                "enum": [
                  "cfed4709-c253-45e8-34b1-08d8e72d932d"
                ],
                "title": "Foam T2542 + Pocket Spring"
              },
              {
                "type": "string",
                "enum": [
                  "b5f04b13-b8d2-45fb-34b2-08d8e72d932d"
                ],
                "title": "Foam T2542 + Pocket Spring + Felt"
              },
              {
                "type": "string",
                "enum": [
                  "d41bbf71-f37a-4c60-34b3-08d8e72d932d"
                ],
                "title": "Foam T2542 + Pocket Spring + Felt + Coconut Mat"
              },
              {
                "type": "string",
                "enum": [
                  "3c1019c2-64b7-49bd-34b4-08d8e72d932d"
                ],
                "title": "Foam T2542 + Profiled Foam Hr"
              },
              {
                "type": "string",
                "enum": [
                  "8b48d5af-3bee-4e5a-34b5-08d8e72d932d"
                ],
                "title": "Foam T2543"
              },
              {
                "type": "string",
                "enum": [
                  "ff44efec-a4a2-4f03-34b6-08d8e72d932d"
                ],
                "title": "Foam T2544"
              },
              {
                "type": "string",
                "enum": [
                  "44e61ee9-283b-419d-34b7-08d8e72d932d"
                ],
                "title": "Foam T2545"
              },
              {
                "type": "string",
                "enum": [
                  "f49f0d3d-0dff-4093-34b8-08d8e72d932d"
                ],
                "title": "Foam T2546"
              },
              {
                "type": "string",
                "enum": [
                  "1cbd5daa-8a35-4783-34b9-08d8e72d932d"
                ],
                "title": "Foam T2547"
              },
              {
                "type": "string",
                "enum": [
                  "51d18d7d-303d-4bce-34ba-08d8e72d932d"
                ],
                "title": "Foam- T30"
              },
              {
                "type": "string",
                "enum": [
                  "444db3c5-f7b9-4d13-34bb-08d8e72d932d"
                ],
                "title": "Foam T3030"
              },
              {
                "type": "string",
                "enum": [
                  "65726a9c-78cd-4737-34bc-08d8e72d932d"
                ],
                "title": "Foam- T3030"
              },
              {
                "type": "string",
                "enum": [
                  "c5bdfb44-a0d7-43c3-34bd-08d8e72d932d"
                ],
                "title": "Foam Visco"
              },
              {
                "type": "string",
                "enum": [
                  "bfd78d7e-c8bc-4f8e-34be-08d8e72d932d"
                ],
                "title": "Foam Visco + Coconut Mat + Foam Hr + Felt + Latex"
              },
              {
                "type": "string",
                "enum": [
                  "f3682ffa-b6c2-46fb-34bf-08d8e72d932d"
                ],
                "title": "Foam Visco + Foam Hr + Foam, T2542"
              },
              {
                "type": "string",
                "enum": [
                  "35c89802-bed9-4632-34c0-08d8e72d932d"
                ],
                "title": "Foam Visco + Foam Hr + Foam, T2543"
              },
              {
                "type": "string",
                "enum": [
                  "3cf98c20-4316-465f-34c1-08d8e72d932d"
                ],
                "title": "Foam Visco + Foam Hr + Foam, T2544"
              },
              {
                "type": "string",
                "enum": [
                  "8eb429ee-dc93-47af-34c2-08d8e72d932d"
                ],
                "title": "Foam Visco + Foam Hr + Foam, T2545"
              },
              {
                "type": "string",
                "enum": [
                  "03f2c058-fea3-432e-34c3-08d8e72d932d"
                ],
                "title": "Foam Visco + Foam Hr + Foam, T2546"
              },
              {
                "type": "string",
                "enum": [
                  "d9865426-acc7-46ec-34c4-08d8e72d932d"
                ],
                "title": "Foam Visco + Foam Hr + Foam, T2547"
              },
              {
                "type": "string",
                "enum": [
                  "8a35e4ac-0af4-4070-34c5-08d8e72d932d"
                ],
                "title": "Foam Visco + Foam Hr + Foam, T2548"
              },
              {
                "type": "string",
                "enum": [
                  "ef0116c8-fc41-47cb-34c6-08d8e72d932d"
                ],
                "title": "Foam Visco + Pocket Spring + Felt + Latex"
              },
              {
                "type": "string",
                "enum": [
                  "bf1c6749-a83c-405a-34c7-08d8e72d932d"
                ],
                "title": "Foam Visco + Profiled Foam Hr + Coconut Mat"
              },
              {
                "type": "string",
                "enum": [
                  "68092cbc-35fe-4bd9-34c8-08d8e72d932d"
                ],
                "title": "Foam-Hr"
              },
              {
                "type": "string",
                "enum": [
                  "497c1b43-192f-419e-34c9-08d8e72d932d"
                ],
                "title": "Foam-Hr + T2540"
              },
              {
                "type": "string",
                "enum": [
                  "43fbf224-6244-4edd-34ca-08d8e72d932d"
                ],
                "title": "Foam-Hr + T3030"
              },
              {
                "type": "string",
                "enum": [
                  "9ee0e709-ab91-43a0-34cb-08d8e72d932d"
                ],
                "title": "Foam-Hr, T21"
              },
              {
                "type": "string",
                "enum": [
                  "a5de0202-fce3-4701-34cc-08d8e72d932d"
                ],
                "title": "Foam-Hr, T25"
              },
              {
                "type": "string",
                "enum": [
                  "da1d13f6-0b31-404d-34cd-08d8e72d932d"
                ],
                "title": "Foam-Hr+T25"
              },
              {
                "type": "string",
                "enum": [
                  "4ed883ef-1e00-45fb-34ce-08d8e72d932d"
                ],
                "title": "Foam-T21+Hr"
              },
              {
                "type": "string",
                "enum": [
                  "01e0cebe-c1ce-4233-34cf-08d8e72d932d"
                ],
                "title": "Foam-T25"
              },
              {
                "type": "string",
                "enum": [
                  "90f2a38a-3ba9-4bef-34d0-08d8e72d932d"
                ],
                "title": "Foam-T30"
              },
              {
                "type": "string",
                "enum": [
                  "913ae7ed-e8fe-4ae8-34d1-08d8e72d932d"
                ],
                "title": "Latex"
              },
              {
                "type": "string",
                "enum": [
                  "fdb3d7b7-adcc-4bd5-34d2-08d8e72d932d"
                ],
                "title": "Latex + Coconut Mat + Felt"
              },
              {
                "type": "string",
                "enum": [
                  "88f46404-062f-4445-34d3-08d8e72d932d"
                ],
                "title": "Latex + Pocket Spring + Felt"
              },
              {
                "type": "string",
                "enum": [
                  "e634a107-37f3-45db-34d4-08d8e72d932d"
                ],
                "title": "Profiled Foam Hr + Bonnell + Felt"
              }
            ]
          },
          "dd914ea6-dcff-4865-8125-08d8e72d92a1": {
            "title": "Frame Material",
            "type": "string",
            "anyOf": [
              {
                "type": "string",
                "enum": [
                  "8d2cfebe-2b27-42ab-34d5-08d8e72d932d"
                ],
                "title": "Metal"
              },
              {
                "type": "string",
                "enum": [
                  "0017c921-2d4c-453b-34d6-08d8e72d932d"
                ],
                "title": "Wood"
              },
              {
                "type": "string",
                "enum": [
                  "d5ec9503-6e22-483b-34d7-08d8e72d932d"
                ],
                "title": "Wood / Steel"
              }
            ]
          },
          "5d32dacf-1a72-41b7-8126-08d8e72d92a1": {
            "title": "Frame Wood Type",
            "type": "string",
            "anyOf": [
              {
                "type": "string",
                "enum": [
                  "449a3d55-25a1-4621-34d8-08d8e72d932d"
                ],
                "title": "Pine"
              },
              {
                "type": "string",
                "enum": [
                  "80392381-0fc8-4e5b-34d9-08d8e72d932d"
                ],
                "title": "Pine / Steel"
              }
            ]
          },
          "b79a36a8-28a7-4a15-8127-08d8e72d92a1": {
            "title": "Leg Material",
            "type": "string",
            "anyOf": [
              {
                "type": "string",
                "enum": [
                  "ee2b44f1-803e-4082-34da-08d8e72d932d"
                ],
                "title": "Metal"
              },
              {
                "type": "string",
                "enum": [
                  "b9e63a81-22de-4aaa-34db-08d8e72d932d"
                ],
                "title": "Plastics"
              },
              {
                "type": "string",
                "enum": [
                  "1582ced1-6929-4a10-34dc-08d8e72d932d"
                ],
                "title": "Steel"
              },
              {
                "type": "string",
                "enum": [
                  "64659ca2-93cc-47a2-34dd-08d8e72d932d"
                ],
                "title": "Wood"
              }
            ]
          },
          "e4fe58c6-3b18-4d9f-8128-08d8e72d92a1": {
            "title": "Unfold Mechanism",
            "type": "string",
            "anyOf": [
              {
                "type": "string",
                "enum": [
                  "a08a8d9b-94e7-4863-34de-08d8e72d932d"
                ],
                "title": "Dl"
              },
              {
                "type": "string",
                "enum": [
                  "6b42cf36-754d-4a0d-34df-08d8e72d932d"
                ],
                "title": "Dolphin"
              },
              {
                "type": "string",
                "enum": [
                  "42027f41-e688-459e-34e0-08d8e72d932d"
                ],
                "title": "Electrically Adjustable Frame By Using The Remote Control"
              },
              {
                "type": "string",
                "enum": [
                  "f592bb16-81d4-4beb-34e1-08d8e72d932d"
                ],
                "title": "Klick-Klack"
              },
              {
                "type": "string",
                "enum": [
                  "d38ea3b5-f5fa-4fb0-34e2-08d8e72d932d"
                ],
                "title": "On Rolls"
              },
              {
                "type": "string",
                "enum": [
                  "0c7d9127-d48c-4c09-34e3-08d8e72d932d"
                ],
                "title": "On Wheels"
              }
            ]
          },
          "6e632bc9-286a-487a-8129-08d8e72d92a1": {
            "title": "Arm Type",
            "type": "string",
            "anyOf": [
              {
                "type": "string",
                "enum": [
                  "fceaccca-cc42-4c11-34e4-08d8e72d932d"
                ],
                "title": "Armless"
              },
              {
                "type": "string",
                "enum": [
                  "ea12f4ef-ef80-45b2-34e5-08d8e72d932d"
                ],
                "title": "Flared Arms"
              },
              {
                "type": "string",
                "enum": [
                  "b226b1c3-6b56-4b26-34e6-08d8e72d932d"
                ],
                "title": "Pillow Top Arms"
              },
              {
                "type": "string",
                "enum": [
                  "bf2f7334-0c16-42e9-34e7-08d8e72d932d"
                ],
                "title": "Recessed Arms"
              },
              {
                "type": "string",
                "enum": [
                  "b763f751-bef3-4ece-34e8-08d8e72d932d"
                ],
                "title": "Round Arms"
              },
              {
                "type": "string",
                "enum": [
                  "f060e532-786a-4c6e-34e9-08d8e72d932d"
                ],
                "title": "Scroll Arms"
              },
              {
                "type": "string",
                "enum": [
                  "ce144554-e083-4b63-34ea-08d8e72d932d"
                ],
                "title": "Square Arm"
              },
              {
                "type": "string",
                "enum": [
                  "5cbee828-95a1-4daf-34eb-08d8e72d932d"
                ],
                "title": "Square Arms"
              },
              {
                "type": "string",
                "enum": [
                  "93c32538-3a79-40eb-34ec-08d8e72d932d"
                ],
                "title": "Square Arms;Scroll Arms"
              }
            ]
          },
          "12264eae-d56d-49f0-812a-08d8e72d92a1": {
            "title": "Back Type",
            "type": "string",
            "anyOf": [
              {
                "type": "string",
                "enum": [
                  "19ed30d4-d8e6-47a2-34ed-08d8e72d932d"
                ],
                "title": "Biscuit Back"
              },
              {
                "type": "string",
                "enum": [
                  "377d02b2-9385-4c2e-34ee-08d8e72d932d"
                ],
                "title": "Button Back"
              },
              {
                "type": "string",
                "enum": [
                  "05e9c734-2f08-40ef-34ef-08d8e72d932d"
                ],
                "title": "Camel Back"
              },
              {
                "type": "string",
                "enum": [
                  "9151d42f-3bfe-4e28-34f0-08d8e72d932d"
                ],
                "title": "Cushion Back"
              },
              {
                "type": "string",
                "enum": [
                  "14f46bc8-32d2-4fd5-34f1-08d8e72d932d"
                ],
                "title": "Fixed Back"
              },
              {
                "type": "string",
                "enum": [
                  "a2ec7d9a-93b8-42ff-34f2-08d8e72d932d"
                ],
                "title": "Fixed Back/Cushion Back"
              },
              {
                "type": "string",
                "enum": [
                  "ac810e1f-7cb7-40b4-34f3-08d8e72d932d"
                ],
                "title": "Loose Back"
              },
              {
                "type": "string",
                "enum": [
                  "a80d40f8-dbef-4ca1-34f4-08d8e72d932d"
                ],
                "title": "Scatter Back"
              }
            ]
          },
          "82122ae0-92f5-494d-812b-08d8e72d92a1": {
            "title": "Style",
            "type": "string",
            "anyOf": [
              {
                "type": "string",
                "enum": [
                  "719096dc-e431-4415-34f5-08d8e72d932d"
                ],
                "title": "Modern"
              },
              {
                "type": "string",
                "enum": [
                  "c7e12085-b3d6-46d8-34f6-08d8e72d932d"
                ],
                "title": "Scandinavian"
              },
              {
                "type": "string",
                "enum": [
                  "2c22ff2a-8b24-48c8-34f7-08d8e72d932d"
                ],
                "title": "Traditional"
              }
            ]
          },
          "fdf78824-3af5-44bf-8120-08d8e72d92a1": {
            "title": "Country",
            "type": "string",
            "anyOf": [
              {
                "type": "string",
                "enum": [
                  "58a4cf00-60a2-45f6-3495-08d8e72d932d"
                ],
                "title": "Poland"
              }
            ]
          },
          "ea39d1e9-0fb1-4b05-812c-08d8e72d92a1": {
            "title": "Approved Use",
            "type": "string",
            "anyOf": [
              {
                "type": "string",
                "enum": [
                  "26a20e65-eb4f-403b-34f8-08d8e72d932d"
                ],
                "title": "Residential Use"
              }
            ]
          },
          "d5aa6a0d-76b1-4afc-812d-08d8e72d92a1": {
            "title": "Level of Assembly",
            "type": "string",
            "anyOf": [
              {
                "type": "string",
                "enum": [
                  "7000a18d-cc80-4f57-34f9-08d8e72d932d"
                ],
                "title": "Partially Assembled"
              }
            ]
          },
          "3649613d-0ec1-462d-812e-08d8e72d92a1": {
            "title": "Warranty",
            "type": "string",
            "anyOf": [
              {
                "type": "string",
                "enum": [
                  "d4987d85-7585-48b7-34fa-08d8e72d932d"
                ],
                "title": "2 Years"
              }
            ]
          }
        },
        "type": "object",
        "properties": {
          "width": {
            "type": "number",
            "title": "Width [cm]"
          },
          "height": {
            "type": "number",
            "title": "Height [cm]"
          },
          "depth": {
            "type": "number",
            "title": "Depth [cm]"
          },
          "shape": {
            "$ref": "#/definitions/a88b5228-7996-40b0-8121-08d8e72d92a1",
            "title": "Shape"
          },
          "orientation": {
            "$ref": "#/definitions/8a88cb57-46ea-4c3c-8122-08d8e72d92a1",
            "title": "Orientation"
          },
          "style": {
            "$ref": "#/definitions/82122ae0-92f5-494d-812b-08d8e72d92a1",
            "title": "Style"
          },
          "primaryColor": {
            "$ref": "#/definitions/7ce2f50f-a659-487e-811e-08d8e72d92a1",
            "title": "Primary Color"
          },
          "secondaryColor": {
            "$ref": "#/definitions/7ce2f50f-a659-487e-811e-08d8e72d92a1",
            "title": "Secondary Color"
          },
          "primaryFabrics": {
            "$ref": "#/definitions/194e3fba-2b3b-4795-811f-08d8e72d92a1",
            "title": "Secondary Color"
          },
          "secondaryFabrics": {
            "$ref": "#/definitions/194e3fba-2b3b-4795-811f-08d8e72d92a1",
            "title": "Secondary Fabrics"
          },
          "upholsteryMaterial": {
            "$ref": "#/definitions/9dc2c5d6-cf0d-4b17-8123-08d8e72d92a1",
            "title": "Upholstery Material"
          },
          "seatAreaWidth": {
            "type": "number",
            "title": "Seat Area Width [cm]"
          },
          "seatAreaHeight": {
            "type": "number",
            "title": "Seat Area Height [cm]"
          },
          "seatAreaDepth": {
            "type": "number",
            "title": "Seat Area Depth [cm]"
          },
          "seatCapacity": {
            "type": "number",
            "title": "Seat Capacity"
          },
          "seatFillMaterial": {
            "$ref": "#/definitions/babea455-179c-4efc-8124-08d8e72d92a1",
            "title": "Seat Fill Material"
          },
          "weightCapacity": {
            "type": "number",
            "title": "Weight Capacity"
          },
          "sleepFunction": {
            "type": "boolean",
            "title": "Sleep Function"
          },
          "sleepAreaWidth": {
            "type": "number",
            "title": "Sleep Area Width [cm]"
          },
          "sleepAreaDepth": {
            "type": "number",
            "title": "Sleep Area Depth [cm]"
          },
          "unfoldMechanism": {
            "$ref": "#/definitions/e4fe58c6-3b18-4d9f-8128-08d8e72d92a1",
            "title": "Unfold Mechanism"
          },
          "cushionsNumber": {
            "type": "number",
            "title": "Number of Cushions"
          },
          "cushionsRemovable": {
            "type": "boolean",
            "title": "Cushions Removable"
          },
          "cushionsUpholsteryMaterial": {
            "$ref": "#/definitions/9dc2c5d6-cf0d-4b17-8123-08d8e72d92a1",
            "title": "Cushions Upholstery Material"
          },
          "cushionsFillMaterial": {
            "$ref": "#/definitions/babea455-179c-4efc-8124-08d8e72d92a1",
            "title": "Cushions Fill Material"
          },
          "tossPillowsIncluded": {
            "type": "boolean",
            "title": "Toss Pillows Included"
          },
          "legWidth": {
            "type": "number",
            "title": "Leg Width [cm]"
          },
          "legHeight": {
            "type": "number",
            "title": "Leg Height [cm]"
          },
          "legDepth": {
            "type": "number",
            "title": "Leg Depth [cm]"
          },
          "legColor": {
            "$ref": "#/definitions/7ce2f50f-a659-487e-811e-08d8e72d92a1",
            "title": "Leg Color"
          },
          "legMaterial": {
            "$ref": "#/definitions/b79a36a8-28a7-4a15-8127-08d8e72d92a1",
            "title": "Leg Material"
          },
          "armType": {
            "$ref": "#/definitions/6e632bc9-286a-487a-8129-08d8e72d92a1",
            "title": "Arm Type"
          },
          "backType": {
            "$ref": "#/definitions/12264eae-d56d-49f0-812a-08d8e72d92a1",
            "title": "Back Type"
          },
          "backFillMaterial": {
            "$ref": "#/definitions/babea455-179c-4efc-8124-08d8e72d92a1",
            "title": "Back Fill Material"
          },
          "isStandalone": {
            "type": "boolean",
            "title": "Standalone"
          },
          "regulatedHeadrest": {
            "type": "boolean",
            "title": "Regulated Headrest"
          },
          "frameMaterial": {
            "$ref": "#/definitions/dd914ea6-dcff-4865-8125-08d8e72d92a1",
            "title": "Frame Material"
          },
          "frameWoodType": {
            "$ref": "#/definitions/5d32dacf-1a72-41b7-8126-08d8e72d92a1",
            "title": "Frame Wood Type"
          },
          "storageContainers": {
            "type": "boolean",
            "title": "Storage Containers"
          },
          "storageContainersNumber": {
            "type": "number",
            "title": "Number of Storage Containers"
          },
          "fireResistant": {
            "type": "boolean",
            "title": "Fire Resistant"
          },
          "approvedUse": {
            "type": "array",
            "uniqueItems": true,
            "title": "Approved Use",
            "items": {
              "$ref": "#/definitions/ea39d1e9-0fb1-4b05-812c-08d8e72d92a1"
            }
          },
          "piecesNumber": {
            "type": "number",
            "title": "Number of Pieces"
          },
          "totalWeight": {
            "type": "number",
            "title": "Total Weight"
          },
          "adultAssemblyRequired": {
            "type": "boolean",
            "title": "Adult Assembly Required"
          },
          "assemblyLevel": {
            "$ref": "#/definitions/d5aa6a0d-76b1-4afc-812d-08d8e72d92a1",
            "title": "Level of Assembly"
          },
          "adultAssemblyNumber": {
            "type": "number",
            "title": "Number of Adults for Assembly"
          },
          "warranty": {
            "$ref": "#/definitions/3649613d-0ec1-462d-812e-08d8e72d92a1",
            "title": "Warranty"
          },
          "countryOfOrigin": {
            "$ref": "#/definitions/fdf78824-3af5-44bf-8120-08d8e72d92a1",
            "title": "Country"
          }
        }
      };

    const categoriesProps = {
        options: props.categories,
        getOptionLabel: (option) => option.name
    };

    const primaryProductsProps = {
        options: props.primaryProducts,
        getOptionLabel: (option) => option.name
    };

    const [state, dispatch] = useContext(Context);

    const stateSchema = {

        id: { value: props.id ? props.id : null, error: "" },
        category: { value: props.categoryId ? props.categories.find((item) => item.id === props.categoryId) : null },
        name: { value: props.name ? props.name : "", error: "" },
        description: { value: props.description ? props.description : "", error: "" },
        sku: { value: props.sku ? props.sku : "", error: "" },
        primaryProduct: { value: props.primaryProductId ? props.primaryProducts.find((item) => item.id === props.primaryProductId) : null },
        images: { value: props.images ? props.images : [] },
        files: { value: props.files ? props.files : [] },
        isNew: { value: props.isNew ? props.isNew : false },
        formData: { value: props.formData ? JSON.parse(props.formData) : {} }
    };

    const stateValidatorSchema = {

        sku: {
            required: {
                isRequired: true,
                error: props.skuRequiredErrorMessage
            }
        },
        name: {
            required: {
                isRequired: true,
                error: props.nameRequiredErrorMessage
            }
        }
    };

    const onCategoryChange = (event, newValue) => {

        dispatch({ type: "SET_IS_LOADING", payload: true });

        setFieldValue({ name: "category", value: newValue });

        const requestOptions = {
            method: "POST",
            headers: { "Content-Type": "application/json" },
            body: JSON.stringify(newValue)
        };

        fetch(props.getCategorySchemaUrl, requestOptions)
            .then(function (response) {

                dispatch({ type: "SET_IS_LOADING", payload: false });

                return response.json().then(jsonResponse => {

                    if (response.ok) {

                        
                    }
                    else {
                        toast.error(props.generalErrorMessage);
                    }
                });
            }).catch(() => {
                dispatch({ type: "SET_IS_LOADING", payload: false });
                toast.error(props.generalErrorMessage);
            });
    };

    const onSubmitForm = (state) => {

        dispatch({ type: "SET_IS_LOADING", payload: true });

        const requestOptions = {
            method: "POST",
            headers: { "Content-Type": "application/json" },
            body: JSON.stringify(state)
        };

        fetch(props.saveUrl, requestOptions)
            .then(function (response) {

                dispatch({ type: "SET_IS_LOADING", payload: false });

                return response.json().then(jsonResponse => {

                    if (response.ok) {

                        setFieldValue({ name: "id", value: jsonResponse.id });
                        toast.success(jsonResponse.message);
                    }
                    else {
                        toast.error(props.generalErrorMessage);
                    }
                });
            }).catch(() => {
                dispatch({ type: "SET_IS_LOADING", payload: false });
                toast.error(props.generalErrorMessage);
            });
    };

    const {
        values,
        errors,
        dirty,
        disable,
        setFieldValue,
        handleOnChange,
        handleOnSubmit
    } = useForm(stateSchema, stateValidatorSchema, onSubmitForm, !props.id);

    const { id, category, sku, name, description, primaryProduct, images, files, isNew, formData } = values;

    return (
        <section className="section section-small-padding product">
            <h1 className="subtitle is-4">{props.title}</h1>
            <div className="columns is-desktop">
                <div className="column is-half">
                    <form className="is-modern-form" onSubmit={handleOnSubmit} method="post">
                        {id &&
                            <input id="id" name="id" type="hidden" value={id} />
                        }
                        <div className="field">
                            <Autocomplete
                                {...categoriesProps}
                                id="category"
                                name="category"
                                fullWidth={true}
                                value={category}
                                onChange={onCategoryChange}
                                autoComplete
                                renderInput={(params) => <TextField {...params} label={props.selectCategoryLabel} margin="normal" />}
                            />
                        </div>
                        <div className="field">
                            <TextField id="sku" name="sku" label={props.skuLabel} fullWidth={true}
                                value={sku} onChange={handleOnChange} helperText={dirty.sku ? errors.sku : ""} error={(errors.sku.length > 0) && dirty.sku} />
                        </div>
                        <div className="field">
                            <TextField id="name" name="name" label={props.nameLabel} fullWidth={true}
                                value={name} onChange={handleOnChange} helperText={dirty.name ? errors.name : ""} error={(errors.name.length > 0) && dirty.name} />
                        </div>
                        <div className="field">
                            <TextField id="description" name="description" label={props.descriptionLabel} fullWidth={true}
                                value={description} onChange={handleOnChange} multiline />
                        </div>
                        <div className="field">
                            <Autocomplete
                                {...primaryProductsProps}
                                id="primaryProductId"
                                name="primaryProductId"
                                fullWidth={true}
                                value={primaryProduct}
                                onChange={(event, newValue) => {
                                    setFieldValue({ name: "primaryProduct", value: newValue });
                                  }}
                                autoComplete
                                renderInput={(params) => <TextField {...params} label={props.selectPrimaryProductLabel} margin="normal" />}
                            />
                        </div>
                        <div className="field">
                            <MediaCloud
                                id="images"
                                name="images"
                                label={props.productPicturesLabel}
                                accept=".png, .jpg"
                                multiple={true}
                                generalErrorMessage={props.generalErrorMessage}
                                deleteLabel={props.deleteLabel}
                                dropFilesLabel={props.dropFilesLabel}
                                dropOrSelectFilesLabel={props.dropOrSelectFilesLabel}
                                files={images}
                                setFieldValue={setFieldValue}
                                saveMediaUrl={props.saveMediaUrl} />
                        </div>
                        <div className="field">
                            <MediaCloud
                                id="files"
                                name="files"
                                label={props.productFilesLabel}
                                accept=".png, .jpg, .pdf, .docx, .zip"
                                multiple={true}
                                generalErrorMessage={props.generalErrorMessage}
                                deleteLabel={props.deleteLabel}
                                dropFilesLabel={props.dropFilesLabel}
                                dropOrSelectFilesLabel={props.dropOrSelectFilesLabel}
                                imagePreviewEnabled={false}
                                files={files}
                                setFieldValue={setFieldValue}
                                saveMediaUrl={props.saveMediaUrl} />
                        </div>
                        <div className="field">
                            <NoSsr>
                                <FormControlLabel
                                    control={
                                    <Switch
                                        onChange={e => {
                                            setFieldValue({ name: "isNew", value: e.target.checked });
                                        }}
                                        checked={isNew}
                                        id="isNew"
                                        name="isNew"
                                        color="secondary" />
                                    }
                                    label={props.isNewLabel} />
                            </NoSsr>
                        </div>
                        <DynamicForm 
                            jsonSchema={jsonSchema} 
                            uiSchema={uiSchema} 
                            formData={formData} 
                            onChange={handleOnChange} />
                        <div className="field">
                            <Button type="submit" variant="contained" color="primary" disabled={state.isLoading || disable}>
                                {props.saveText}
                            </Button>
                        </div>
                    </form>
                    {state.isLoading && <CircularProgress className="progressBar" />}
                </div>
            </div>
        </section>
    );
}

ProductForm.propTypes = {
    id: PropTypes.string,
    categoryId: PropTypes.string,
    name: PropTypes.string,
    sku: PropTypes.string,
    primaryProductId: PropTypes.string,
    images: PropTypes.array,
    files: PropTypes.array,
    isNewLabel: PropTypes.string.isRequired,
    selectCategoryLabel: PropTypes.string.isRequired,
    selectPrimaryProductLabel: PropTypes.string.isRequired,
    productFilesLabel: PropTypes.string.isRequired,
    productPicturesLabel: PropTypes.string.isRequired,
    skuLabel: PropTypes.string.isRequired,
    nameLabel: PropTypes.string.isRequired,
    descriptionLabel: PropTypes.string.isRequired,
    saveText: PropTypes.string.isRequired,
    categories: PropTypes.array.isRequired,
    primaryProducts: PropTypes.array,
    dropOrSelectFilesLabel: PropTypes.string.isRequired,
    dropFilesLabel: PropTypes.string.isRequired,
    saveMediaUrl: PropTypes.string.isRequired,
    deleteLabel: PropTypes.string.isRequired,
    getCategorySchemaUrl: PropTypes.string.isRequired,
    generalErrorMessage: PropTypes.string.isRequired
};

export default ProductForm;
