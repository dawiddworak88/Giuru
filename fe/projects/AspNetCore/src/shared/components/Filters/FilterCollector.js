import React, { useState } from "react";
import PropTypes from "prop-types";
import { Accordion, AccordionDetails, AccordionSummary, Button, Checkbox, Chip, Drawer, ListItemText, MenuItem, Select, Typography } from "@mui/material";
import Arrow from "../../Icons/Arrow";
import { CheckBox, CheckBoxChecked } from "../../Icons/CheckBox";
import Remove from "../../Icons/Remove";
import Filters from "../../Icons/Filters";
import { Close } from "@mui/icons-material";

function FilterCollector(props) {
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

    return (
        <div>
            <div className="filters-collector is-flex is-align-items-center">
                {props.total !== 0 &&
                    <div className="filters-collector__total pr-5">
                        <p>{props.total} {props.resultsLabel}</p>
                    </div>
                }
                {props.filterInputs && props.filterInputs.length > 0 &&
                    <div className="filters-collector__filters">
                        {props.filterInputs.map((item, index) => (
                            <Select
                                key={index}
                                multiple displayEmpty
                                value={props.filters}
                                onChange={handleOnSelectFiltersChange}
                                MenuProps={{
                                    PaperProps: {
                                        sx: {
                                            padding: '1.5rem 1.25rem 0 1.25rem'
                                        },
                                    }
                                }}
                                renderValue={() => {
                                    return (
                                    <Typography fontWeight={700}>
                                        {item.label}
                                    </Typography>
                                    );
                                }}
                                IconComponent={(props) => <Arrow {...props}/>}
                                className="filters-collector__filters__select mr-3 py-0 px-2"
                            >
                                {item.items.map((variant, index) => (
                                    <MenuItem
                                        className="filters-collector__filters__select__item pt-0 pr-4 pb-5 pl-0"
                                        value={{key: item.key, value: variant.value, label: variant.label }}
                                        key={index}
                                    >
                                        <Checkbox
                                            className="filters-collector__filters__select__item__checkbox"
                                            checked={isFilterSelected(item.key, variant.value)}
                                            icon={<CheckBox />}
                                            checkedIcon={<CheckBoxChecked />} />
                                        <ListItemText primary={variant.label} />
                                    </MenuItem>
                                ))}
                            </Select>
                        ))}
                        <Button
                            className="filters-collector__filters__button px-4 py-1"
                            onClick={() => setSidebarOpen(true)}
                            endIcon={<Filters />}
                        >
                            <Typography fontWeight={700}>
                                {props.allFilters}
                            </Typography>
                        </Button>
                    </div>
                }
                {props.sortItems && props.sortItems.length > 0 &&
                    <div className="is-flex filters-collector__sort">
                        <div className="filters-collector__sort__text has-text-weight-bold mr-3">{props.sortLabel}</div>
                        <Select
                            className="filters-collector__sort__select"
                            IconComponent={(props) => <Arrow {...props}/>}
                            value={props.sorting}
                            onChange={(e) => props.setSorting(e.target.value)}
                        >
                            {props.sortItems.map((item, index) => (
                                <MenuItem
                                    className="py-2 px-5"
                                    key={index}
                                    value={item.key}
                                >
                                    {item.label}    
                                </MenuItem>
                            ))}
                        </Select>
                    </div>
                }
            </div>
            {props.total !== 0 &&
                <div className="filters-collector__total-mobile">
                    <p>{props.total} {props.resultsLabel}</p>
                </div>
            }
            <div className="active-filters">
                {props.filters.map((item, index) => (
                    <Chip
                        className="active-filters__item pr-3 mr-3 mb-3"
                        key={index}
                        label={item.label}
                        onDelete={() => handleOnDeleteFilter(index)}
                        deleteIcon={<Remove />} />
                ))}
                {props.filters.length > 1 && 
                    <Button
                        className="active-filters__button button-clear px-3 py-1 mb-3 has-text-weight-bold"
                        onClick={handleOnClickClearFilters}
                    >{props.clearAllFilters}</Button>
                }
            </div>
            <Drawer
                anchor="right"
                open={sidebarOpen}
                onClose={() => setSidebarOpen(false)}
            >
                <div className="sidebar is-flex is-flex-direction-column">
                    <div className="is-flex is-justify-content-space-between">
                        <Typography fontWeight={700} fontSize={20}>
                            {props.filtersLabel}
                        </Typography>
                        <Button
                            className="sidebar__header__button"
                            onClick={() => setSidebarOpen(false)}
                        >
                            <Close />
                        </Button>
                    </div>
                    <div className="sidebar__filters">
                        {props.filterInputs && props.filterInputs.length > 0 && props.filterInputs.map((item, index) => (
                            <Accordion key={index} className="sidebar__filters__filter">
                                <AccordionSummary
                                    expandIcon={<Arrow />}
                                >
                                    {item.label}
                                </AccordionSummary>
                                {item.items.map((variant, index) => (
                                    <AccordionDetails key={index} className="sidebar__filters__filter__item is-flex is-align-items-center">
                                        <Checkbox
                                            className="sidebar__filters__filter__item__checkbox"
                                            checked={isFilterSelected(item.key, variant.value)}
                                            icon={<CheckBox />}
                                            checkedIcon={<CheckBoxChecked />}
                                            onClick={() => handleOnSidebarFilterSet(item.key, variant.value, variant.label) } />
                                        <Typography>{variant.label}</Typography>
                                    </AccordionDetails>
                                ))}
                            </Accordion>
                        ))}
                    </div>
                    <div className="sidebar__fotter is-flex is-justify-content-space-between">
                        <Button
                            className="sidebar__fotter__button button-clear py-3"
                            onClick={handleOnClickClearFilters}
                            disabled={!props.filters.length > 0}
                        >
                            <Typography fontWeight={700}>
                                {props.clearAllFilters}
                            </Typography>
                        </Button>
                        <Button
                            className="sidebar__fotter__button py-3"
                            onClick={() => setSidebarOpen(false)}
                        >
                            <Typography fontWeight={700}>
                                {props.seeResult}
                            </Typography>
                        </Button>
                    </div>
                </div>
            </Drawer>
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