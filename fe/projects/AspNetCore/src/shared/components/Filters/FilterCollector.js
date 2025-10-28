import React, { useState } from "react";
import PropTypes from "prop-types";
import { 
    Accordion, AccordionDetails, AccordionSummary, Box, Button, Checkbox, 
    Chip, Drawer, IconButton, ListItemText, MenuItem, Select, Stack, Typography, 
    useMediaQuery, useTheme
} from "@mui/material";
import { Close } from "@mui/icons-material";
import { 
    ArrowIcon, CheckboxCheckedIcon, CheckboxIcon,
    FiltersIcon, RemoveIcon 
} from "../../icons";
import NestedFilterAccordionDropdown from "./NestedFilterAccordionDropdown";

const FilterCollector = (props) => {
    const [sidebarOpen, setSidebarOpen] = useState(false)

    const handleOnSelectFiltersChange = (event) => {
        const {
            target: { value }
        } = event

        const newItem = value[value.length - 1]

        updateFilters(newItem.key, newItem.value, newItem.label)
    }

    const handleOnSidebarFilterSet = (key, value, label) => updateFilters(key, value, label)

    const updateFilters = (key, value, label) => {
        if (isFilterSelected(key, value)) {
            props.setFilters(props.filters.filter(x => !(x.value === value && x.key === key)))
        } else {
            props.setFilters(props.filters.concat({ key, value, label}))
        }
    }
    
    const isFilterSelected = (key, value) => {
        return props.filters.find(x => x.key === key && x.value === value) ? true : false
    }
    
    const handleOnDeleteFilter = (indexToRemove) => {
        props.setFilters(props.filters.filter((_, index) => index !== indexToRemove))
    }

    const handleOnClickClearFilters = () => {
        props.setFilters([])
    }

    const theme = useTheme();
    const isMobile = useMediaQuery(theme.breakpoints.down('md'));
    const isNotMobile = !isMobile;

    return (
        <div>
            <Box 
                component="div"
                sx={{
                    display: 'flex',
                    alignItems: 'center',
                    width: '100%',
                    mb: '1.5rem'
                }}
            >
                {props.total !== 0 && (
                    <Box
                        component="div"
                        sx={{
                            marginRight: "1.5rem"
                        }}
                    >
                        <Typography
                            sx={{
                                fontSize: "0.875rem",
                                fontWeight: 400,
                                lineHeight: "1.5rem",
                                color: "black.300"
                            }}
                        >
                            {props.total} {props.resultsLabel}
                        </Typography>
                    </Box>
                )}
                {isNotMobile &&
                    <Box
                        component="div"
                        sx={{
                            display: "flex",
                            alignItems: "center",
                            gap: "1.5rem"
                        }}
                    >
                        {props.filterInputs && props.filterInputs.length > 0 && (
                            <Box
                                sx={{
                                    display: "flex",
                                    gap: "0.75rem",
                                    marginRight: "0.75rem"
                                }}
                            >
                                {props.filterInputs.map((item, index) => (
                                    item.isNested ? (
                                        <NestedFilterAccordionDropdown 
                                            label={item.label}
                                            filterGroups={item.items}
                                            selectedFilters={props.filters}
                                            onChange={updateFilters}
                                            asDropdown={true}
                                        />
                                    ) : (
                                        <Select
                                            key={index}
                                            multiple
                                            displayEmpty
                                            value={props.filters}
                                            sx={{
                                                height: "2.5rem",
                                                borderRadius: "0.25rem",
                                                backgroundColor: "gray.100",
                                                paddingX: "1rem",
                                                paddingY: "0.25rem",
                                                boxShadow: "none",
                                                color: "blackBase",
                                                fontWeight: 700,
                                                "&& .MuiSelect-select": {
                                                    display: "flex",
                                                    alignItems: "center",
                                                    minHeight: "unset",
                                                    border: "unset",
                                                    padding: 0,
                                                    paddingRight: "calc(0.8125rem + 0.625rem)",
                                                    gap: "10px"
                                                },
                                                "&& .MuiOutlinedInput-notchedOutline": {
                                                    border: "unset"
                                                },
                                                "&& .MuiSelect-icon": {
                                                    right: "1rem",  
                                                    color: "currentColor",
                                                },
                                                "&&:hover, &&:has(.MuiSelect-select[aria-expanded='true'])": {
                                                    backgroundColor: "mint.500",
                                                    "&& .MuiSelect-select": {
                                                        color: "whiteBase"
                                                    },
                                                    "&& .MuiSelect-icon": {
                                                        color: "whiteBase"
                                                    }
                                                }
                                            }}
                                            MenuProps={{
                                                autoFocus: false,
                                                PaperProps: {
                                                    sx: {
                                                        marginTop: "1rem",
                                                        borderRadius: "0.5rem",
                                                        paddingX: "2.5rem",
                                                        paddingY: "2rem",
                                                        width: "20rem",
                                                        maxHeight: "17.25rem",
                                                        overflowY: "auto"
                                                    }
                                                }
                                            }}
                                            onChange={handleOnSelectFiltersChange}
                                            IconComponent={(props) => <ArrowIcon {...props}/>}
                                            renderValue={() => {
                                                return (
                                                    <Typography 
                                                        sx={{
                                                            display: "flex",
                                                            alignItems: "center",
                                                            fontWeight: 700,
                                                            fontSize: "0.75rem",
                                                            color: "inherit"
                                                        }}
                                                    >
                                                        {item.label}
                                                    </Typography>
                                                );
                                            }}
                                        >
                                            {item.items.map((variant, index) => (
                                                <MenuItem
                                                    disableRipple
                                                    key={index}
                                                    value={{
                                                        key: item.key, 
                                                        value: variant.value, 
                                                        label: variant.label 
                                                    }}
                                                    sx={{
                                                        height: "1.5rem",
                                                        padding: 0,
                                                        fontSize: "0.875rem",
                                                        fontWeight: 400,
                                                        color: "blackBase",
                                                        "&&:hover": {
                                                            backgroundColor: "whiteBase"
                                                        },
                                                        "&:not(:first-child)": {
                                                            marginTop: "1.5rem"
                                                        }
                                                    }}
                                                >
                                                    <Checkbox
                                                        disableRipple
                                                        checked={isFilterSelected(item.key, variant.value)}
                                                        icon={<CheckboxIcon />}
                                                        checkedIcon={<CheckboxCheckedIcon />} 
                                                        sx={{
                                                            padding: 0,
                                                            height: "1.5rem",
                                                            "&&:hover": {
                                                               backgroundColor: "whiteBase"
                                                            }
                                                        }}
                                                    />
                                                    {variant.label}
                                                </MenuItem>
                                            ))}
                                        </Select>
                                    )
                                ))}
                            </Box>
                        )}
                    </Box>
                }
                {props.filterInputs && props.filterInputs.length > 0 &&
                    <Button
                        disableRipple
                        onClick={() => setSidebarOpen(true)}
                        endIcon={<FiltersIcon />}
                        sx={{
                            height: "2.5rem",
                            paddingX: "1rem",
                            paddingY: "0.25rem",
                            borderRadius: "0.25rem",
                            backgroundColor: "gray.100",
                            display: "flex",
                            gap: "0.625rem",
                            fontWeight: 700,
                            color: "blackBase",
                            fontSize: "0.75rem",
                            "&&:hover": {
                                backgroundColor: "mint.500",
                                color: "whiteBase"
                            }
                        }}
                    >
                        {props.allFilters}
                    </Button>
                }
                {props.sortItems && props.sortItems.length > 0 &&
                    <Box 
                        sx={{
                            display: "flex",
                            minWidth: "fit-content",
                            alignItems: "center",
                            marginLeft: "auto",
                            gap: "1.25rem"
                        }}
                    >
                        <Typography
                            sx={{
                                fontSize: "0.75rem",
                                fontWeight: 700,
                                color: "blackBase"
                            }}
                        >
                            {props.sortLabel}
                        </Typography>
                        <Select
                            IconComponent={(props) => <ArrowIcon {...props}/>}
                            value={props.sorting}
                            onChange={(e) => props.setSorting(e.target.value)}
                            sx={{
                                paddingX: "9px",
                                paddingY: "0.25rem",
                                "&& .MuiSelect-select": {
                                    minHeight: "unset",
                                    padding: 0,
                                    paddingRight: "calc(0.8125rem + 0.625rem)",
                                    gap: "10px"
                                },
                                "&& .MuiOutlinedInput-notchedOutline": {
                                    border: "unset"
                                },
                                "&& .MuiSelect-icon": {
                                    right: "9px",  
                                    color: "blackBase"
                                },
                            }}
                            MenuProps={{
                                PaperProps: {
                                    autoFocus: false,
                                    sx: {
                                        marginTop: "1rem",
                                        paddingX: "1.5rem",
                                        paddingY: "1rem",
                                        borderRadius: "0.25rem",
                                        "&.MuiMenuItem-root.Mui-selected, &.MuiMenuItem-root.Mui-selected:hover": {
                                            backgroundColor: "whiteBase",
                                            color: "mint.500"
                                        },
                                    }
                                }
                            }}
                            renderValue={(value) => {
                                return (
                                    <Typography
                                        sx={{
                                            fontSize: "0.75rem",
                                            fontWeight: 700,
                                            color: "mint.500"
                                        }}
                                    >
                                        {props.sortItems.find(item => item.key === value)?.label}
                                    </Typography>
                                )
                            }}
                        >
                            {props.sortItems.map((item, index) => (
                                <MenuItem
                                    disableRipple
                                    key={index}
                                    value={item.key}
                                    sx={{
                                        fontSize: "0.875rem",
                                        fontWeight: 400,
                                        color: "blackBase",
                                        "&&:hover": {
                                            backgroundColor: "whiteBase"
                                        },
                                        "&:not(:first-child)": {
                                            marginTop: "1rem"
                                        }
                                    }}
                                >
                                    {item.label}    
                                </MenuItem>
                            ))}
                        </Select>
                    </Box>
                }
            </Box>
            {props.filters && props.filters.length > 0 && (
                <Box 
                    sx={{
                        display: 'flex',
                        gap: '0.75rem',
                        marginBottom: "2.5rem",
                        flexWrap: 'wrap'
                    }}
                >
                    {props.filters.map((selectedFilter, index) => (
                        <Chip
                            key={index}
                            label={selectedFilter.label}
                            onDelete={() => handleOnDeleteFilter(index)}
                            deleteIcon={<RemoveIcon />} 
                            sx={{
                                paddingY: "0.25rem",
                                paddingX: "1rem",
                                display: "flex",
                                gap: "0.5rem",
                                width: "fit-content",
                                borderRadius: "0.25rem",
                                backgroundColor: "mint.500",
                                border: 1,
                                borderStyle: "solid",
                                borderColor: "mint.500",
                                color: "whiteBase",
                                fontSize: "0.75rem",
                                fontWeight: 700,
                                "& .MuiChip-label": {
                                    padding: 0
                                },
                                "& .MuiChip-deleteIcon": {
                                    margin: 0
                                },
                                "&:hover": {
                                    backgroundColor: "mint.400"
                                }
                            }}
                        />
                    ))}
                    {props.filters.length > 1 && 
                        <Button
                            onClick={handleOnClickClearFilters}
                            sx={{
                                paddingY: "0.25rem",
                                paddingX: "1rem",
                                borderRadius: "0.25rem",
                                backgroundColor: "black.100",
                                border: 1,
                                borderStyle: "solid",
                                borderColor: "gray.300",
                                color: "blackBase",
                                fontSize: "0.75rem",
                                fontWeight: 700,
                                "&:hover": {
                                    backgroundColor: "mint.500",
                                    borderColor: "mint.500",
                                    color: "whiteBase"
                                }
                            }}
                        >
                            {props.clearAllFilters}
                        </Button>
                    }
                </Box>   
            )}
            {props.filterInputs && props.filterInputs.length > 0 && (
                <Drawer
                    anchor="right"
                    open={sidebarOpen}
                    onClose={() => setSidebarOpen(false)}
                    sx={{
                        minHeight: "100vh",
                    }}
                >
                    <Box 
                        sx={{
                            display: "flex",
                            flexDirection: "column",
                            height: "100%",
                            minWidth: "28.875rem"
                        }}
                    >
                        <Box 
                            sx={{
                                display: "flex",
                                justifyContent: "space-between",
                                alignItems: "center",
                                padding: "0 1.875rem 0 1.5rem",
                                minHeight: "5rem",
                                flexShrink: 0
                            }}
                        >
                            <Typography 
                                sx={{
                                    fontSize: "1.25rem",
                                    fontWeight: 700,
                                    color: "blackBase"
                                }}    
                            >
                                {props.filtersLabel}
                            </Typography>
                            <IconButton
                                disableRipple
                                disableTouchRipple
                                disableElevation
                                onClick={() => setSidebarOpen(false)}
                                sx={{
                                    color: "blackBase"
                                }}
                            >
                                <Close />
                            </IconButton>
                        </Box>
                        <Box 
                            sx={{
                                paddingX: "1.5rem",
                                flex: 1,
                                overflowY: "auto"
                            }}
                        >
                            {props.filterInputs.map((item, index) => (
                                item.isNested ? (
                                    <NestedFilterAccordionDropdown 
                                        key={index}
                                        label={item.label}
                                        filterGroups={item.items}
                                        selectedFilters={props.filters}
                                        onChange={updateFilters}
                                    />
                                ) : (
                                    <Accordion 
                                        key={index}
                                        elevation={0}
                                        disableGutters
                                        square
                                        sx={{
                                            border: 1,
                                            borderStyle: "solid",
                                            borderColor: "gray.300",
                                            "&&:before": {
                                                display: "none"
                                            }
                                        }}
                                    >
                                        <AccordionSummary
                                            expandIcon={<ArrowIcon />}
                                            sx={{
                                                paddingY: "1rem",
                                                fontWeight: 700,
                                                fontSize: "0.875rem",
                                                color: "blackBase",
                                                lineHeight: "1.5rem",
                                                letterSpacing: "0.14px"
                                            }}
                                        >
                                            {item.label}
                                        </AccordionSummary>
                                        <AccordionDetails 
                                            sx={{
                                                paddingX: 0,
                                                marginBottom: "1.5rem"
                                            }}
                                        >
                                            <Stack spacing="1.5rem">
                                                {item.items.map((variant, index) => (
                                                    <Stack 
                                                        key={index}
                                                        direction="row"
                                                        sx={{
                                                            fontSize: "0.875rem",
                                                            fontWeight: 400,
                                                            color: "blackBase",
                                                            alignItems: "center"
                                                        }}
                                                    >
                                                        <Checkbox
                                                            disableRipple
                                                            checked={isFilterSelected(item.key, variant.value)}
                                                            icon={<CheckboxIcon />}
                                                            checkedIcon={<CheckboxCheckedIcon />}
                                                            onClick={() => handleOnSidebarFilterSet(item.key, variant.value, variant.label)}
                                                            sx={{
                                                                height: "1.5rem"
                                                            }} 
                                                        />
                                                        {variant.label}
                                                    </Stack>
                                                ))}    
                                            </Stack>        
                                        </AccordionDetails>
                                </Accordion>
                            )))}
                        </Box>
                        <Box 
                            sx={{
                                padding: "1.5rem",
                                display: "flex",
                                gap: "0.5rem",
                                flexShrink: 0
                            }}
                        >
                            <Button
                                onClick={handleOnClickClearFilters}
                                disabled={!props.filters.length > 0}
                                sx={{
                                    borderRadius: "0.25rem",
                                    border: 1,
                                    borderColor: "gray.300",
                                    color: "blackBase",
                                    width: "100%",
                                    paddingY: "0.5rem",
                                    paddingX: "1.5rem",
                                    maxHeight: "3.125rem",
                                    fontWeight: 700,
                                    fontSize: "0.875rem",
                                    lineHeight: "1.5rem",
                                    "&&:disabled": {
                                        color: "black.300",
                                        backgroundColor: "gray.200"
                                    }
                                }}
                            >
                                {props.clearAllFilters}
                            </Button>
                            <Button
                                onClick={() => setSidebarOpen(false)}
                                sx={{
                                    width: "100%",
                                    backgroundColor: "mint.500",
                                    borderRadius: "0.25rem",
                                    paddingY: "0.5rem",
                                    paddingX: "1.5rem",
                                    maxHeight: "3.125rem",
                                    fontWeight: 700,
                                    fontSize: "0.875rem",
                                    lineHeight: "1.5rem",
                                    color: "whiteBase",
                                    "&&:hover": {
                                        backgroundColor: "mint.400"
                                    }
                                }}
                            >
                                {props.seeResult}
                            </Button>
                        </Box>
                    </Box>
                </Drawer>
            )}
        </div>
    )
}

FilterCollector.propTypes = {
    sortLabel: PropTypes.string,
    sortItems: PropTypes.array,
    total: PropTypes.number,
    resultsLabel: PropTypes.string,
    filterInputs: PropTypes.array,
    allFiltersLabel: PropTypes.string,
    clearAllFiltersLabel: PropTypes.string,
    filtersLabel: PropTypes.string,
    seeResultLabel: PropTypes.string
}

export default FilterCollector;