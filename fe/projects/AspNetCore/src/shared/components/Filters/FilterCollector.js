import { useState } from "react";
import PropTypes from "prop-types";
import { Button, Checkbox, Chip, Drawer, ListItemText, MenuItem, Select, Typography } from "@mui/material";
import Arrow from "../../Icons/Arrow";
import { CheckBox, CheckBoxChecked } from "../../Icons/CheckBox";
import Close from "../../Icons/Close";
import Filtres from "../../Icons/Filtres";

function FilterCollector(props) {
    const [sortValue, setSortValue] = useState(props.sortItems && props.sortItems.length > 0 ? props.sortItems[0].key : 0)
    const [filters, setFilters] = useState([])
    const [sidebarOpen, setSidebarOpen] = useState(false)

    const handleOnFiltersChange = (event) => {
        const {
            target: { value }
        } = event

        const newItem = value[value.length - 1]
        const exists = isFilterSelected(newItem.key, newItem.value)

        if (exists) {
            setFilters(filters.filter(x => !(x.value === newItem.value && x.key === newItem.key)))
        } else {
            setFilters(value)
        }
    }

    const handleOnDeleteFilter = (indexToRemove) => {
        setFilters(filters.filter((_, index) => index !== indexToRemove))
    }

    const handleOnClickClearFiltres = () => {
        setFilters([]);
    }

    const isFilterSelected = (key, value) => {
        return filters.find(x => x.key === key && x.value === value) ? true : false
    }

    const handleOnSortChange = (value) => {
        setSortValue(value)
        // fetch sorted items
    }

    return (
        <div>
            <div className="filters-collector is-flex is-align-items-center">
                {props.total &&
                    <div className="filters-collector__total pr-5">
                        <p>{props.total} {props.resultsLabel}</p>
                    </div>
                }
                {props.filterItems && props.filterItems.length > 0 &&
                    <div className="filters-collector__filtres">
                        {props.filterItems.map((item, index) => (
                            <Select
                                key={index}
                                multiple displayEmpty
                                value={filters}
                                onChange={handleOnFiltersChange}
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
                                className="filters-collector__filtres__select mr-3 py-0 px-2"
                            >
                                {item.variants.map((variant, index) => (
                                    <MenuItem
                                        className="filters-collector__filtres__select__item pt-0 pr-4 pb-5 pl-0"
                                        value={{key: item.key, value: variant.value, label: variant.label }}
                                        key={index}
                                        disableRipple>
                                        <Checkbox
                                            className="filters-collector__filtres__select__item__checkbox"
                                            disableRipple disableFocusRipple
                                            checked={isFilterSelected(item.key, variant.value)}
                                            icon={<CheckBox />}
                                            checkedIcon={<CheckBoxChecked />} />
                                        <ListItemText primary={variant.label} />
                                    </MenuItem>
                                ))}
                            </Select>
                        ))}
                        <Button
                            className="filters-collector__filtres__button px-4 py-1"
                            onClick={() => setSidebarOpen(true)}
                            disableRipple
                            disableFocusRipple
                            disableTouchRipple
                        >
                            <Typography fontWeight={700} className="pr-2">
                                {props.allFiltresLabel}
                            </Typography>
                            <Filtres />
                        </Button>
                    </div>
                }
                {props.sortItems && props.sortItems.length > 0 &&
                    <div className="is-flex filters-collector__sort">
                        <div className="filters-collector__sort__text has-text-weight-bold">{props.sortLabel}</div>
                        <Select
                            className="filters-collector__sort__select"
                            IconComponent={(props) => <Arrow {...props}/>}
                            value={sortValue}
                            onChange={(e) => handleOnSortChange(e.target.value)}
                        >
                            {props.sortItems.map((item, index) => (
                                <MenuItem
                                    className="py-2 px-5"
                                    key={index}
                                    value={item.key}
                                    disableRipple
                                >
                                    {item.label}    
                                </MenuItem>
                            ))}
                        </Select>
                    </div>
                }
            </div>
            {props.total &&
                <div className="filters-collector__total-mobile">
                    <p>{props.total} {props.resultsLabel}</p>
                </div>
            }
            <div className="active-filtres">
                {filters.map((item, index) => (
                    <Chip
                        className="active-filtres__item pr-3 mr-3 mb-3"
                        key={index}
                        label={item.label}
                        onDelete={() => handleOnDeleteFilter(index)}
                        deleteIcon={<Close />} />
                ))}
                {filters.length > 1 && 
                    <Button
                        className="active-filtres__button px-3 py-1 mb-3 has-text-weight-bold"
                        onClick={handleOnClickClearFiltres}
                        disableRipple
                        disableTouchRipple
                        disableFocusRipple
                    >{props.clearAllFiltresLabel}</Button>
                }
            </div>
            <Drawer
                className="filters-collector__sidebar"
                anchor="right"
                open={sidebarOpen}
                onClose={() => setSidebarOpen(false)}
            >
                <p>To jest sidebar z filtrami</p>
            </Drawer>
        </div>
    )
}

FilterCollector.propTypes = {
    sortLabel: PropTypes.string,
    sortItems: PropTypes.array,
    total: PropTypes.number,
    resultsLabel: PropTypes.string,
    filterItems: PropTypes.array,
    allFiltresLabel: PropTypes.string,
    clearAllFiltresLabel: PropTypes.string,
    filtresLabel: PropTypes.string
}

export default FilterCollector;