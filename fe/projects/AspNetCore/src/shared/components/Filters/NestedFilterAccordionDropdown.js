import React, { useState } from "react";
import { ArrowIcon, CheckboxCheckedIcon, CheckboxIcon } from "../../icons";
import { Accordion, AccordionDetails, AccordionSummary, Box, Button, Checkbox, Divider, FormControlLabel, Paper, Popover, Stack } from "@mui/material";

const NestedFilterAccordionDropdown = ({
    label,
    filterGroups,
    selectedFilters,
    onChange,
    asDropdown = false
}) => {
    const [isPopoverOpen, setIsPopoverOpen] = useState(false);
    const [popoverAnchorElement, setPopoverAnchorElement] = useState(null);

    const handleButtonClick = (event) => {
        setPopoverAnchorElement(event.currentTarget);
        setIsPopoverOpen(true);
    };

    const handlePopoverClose = () => {
        setPopoverAnchorElement(null);
        setIsPopoverOpen(false);
    };

    const Filter = ({
        filterGroup,
        index,
        expandAsDefault = false
    }) => {
        return (
            <Accordion
                elevation={0}
                key={index}
                disableGutters
                square
                defaultExpanded={expandAsDefault}
            >
                <AccordionSummary
                    expandIcon={expandAsDefault ? null : <ArrowIcon />}
                    sx={{
                        fontWeight: 700,
                        fontSize: "0.875rem",
                        color: "#171717",
                        ...(expandAsDefault && {
                            paddingX: 0
                        })
                    }}
                >
                    {filterGroup.label}
                </AccordionSummary>
                <AccordionDetails
                    sx={{
                        ...(expandAsDefault && {
                            paddingX: 0
                        })
                    }}
                >
                    <Stack>
                        {filterGroup.items.map((item, itemIndex) => (
                            <FormControlLabel 
                                key={itemIndex}
                                control={
                                    <Checkbox
                                        icon={<CheckboxIcon />}
                                        checkedIcon={<CheckboxCheckedIcon />}
                                        onChange={() => onChange(filterGroup.key, item.value, `${filterGroup.label} ${item.label}`)}
                                        sx={{
                                            padding: 0,
                                            height: "1.5rem"
                                        }}
                                    />
                                }
                                label={item.label}
                                componentsProps={{
                                    typography: {
                                        sx: {
                                            marginLeft: "0.5rem",
                                            fontWeight: 400,
                                            fontSize: "0.875rem"
                                        }
                                    }
                                }}
                                sx={{
                                    height: "1.5rem",
                                    padding: 0,
                                    "&:not(:first-child)": {
                                        marginTop: "1.5rem"
                                    }
                                }}
                            />
                        ))}
                    </Stack>
                </AccordionDetails>
            </Accordion>
        )
    }

    return (
        <Box>
            {asDropdown ? (
                <div>
                    <Button
                    variant="contained"
                    disableElevation
                    onClick={event => handleButtonClick(event)}
                    sx={{
                        height: "2.5rem",
                        borderRadius: "0.25rem",
                        paddingX: "1rem",
                        paddingY: "0.25rem",
                        backgroundColor: "#F7F7F7",
                        color: "#171717",
                        fontWeight: 700,
                        fontSize: "0.75rem"
                    }}
                >
                    {label}
                </Button>
                <Popover
                    open={isPopoverOpen}
                    onClose={handlePopoverClose}
                    anchorEl={popoverAnchorElement}
                    anchorOrigin={{
                        vertical: 'bottom',
                        horizontal: 'center'
                    }}
                    transformOrigin={{
                        vertical: 'top',
                        horizontal: 'center'
                    }}
                    slotProps={{
                        paper: {
                            sx: { 
                                marginTop: "1rem",
                                borderRadius: "0.5rem",
                                paddingX: "2.5rem",
                                paddingY: "2rem"
                            }
                        }
                    }}
                >
                    <Paper
                        elevation={0}
                        sx={{
                            minWidth: "20rem",
                            padding: 0
                        }}
                    >
                        {filterGroups.map((group, index) => (
                            <Filter
                                filterGroup={group}
                                index={index}
                            />
                        ))}
                    </Paper>
                </Popover>
                </div>
        ) : (
            <Accordion
                elevation={0}
                // key={index}
                disableGutters
                square
            >
                <AccordionSummary
                    expandIcon={<ArrowIcon />}
                >
                    {label}
                </AccordionSummary>
                <AccordionDetails>
                    {filterGroups.map((group, index) => (
                        <Filter
                            filterGroup={group}
                            index={index}
                            expandAsDefault={true}
                        />
                    ))}
                </AccordionDetails>
            </Accordion>
            )
        }
        </Box>
    )
}

export default NestedFilterAccordionDropdown;